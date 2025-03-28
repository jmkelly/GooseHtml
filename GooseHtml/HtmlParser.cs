using GooseHtml.Attributes;

namespace GooseHtml;

public sealed class HtmlParser(string html) 
{
	private readonly string _html = html;
	private ReadOnlySpan<char> HtmlSpan => _html.AsSpan();
	private int Length => HtmlSpan.Length;
	private static ReadOnlySpan<char> CommentOpen => "<!--".AsSpan();
	private static ReadOnlySpan<char> CommentClose => "-->".AsSpan();

	private const char BackTick = '`';
	private const char TagOpen = '<';
	private const char TagEnd = '>';
	private const char Equal = '=';
	private const char DoubleQuote = '"';
	private const char SingleQuote = '\'';
	private const char BackSlash = '\n';
	private static ReadOnlySpan<char> MalformedEndTag => "</";
	private static ReadOnlySpan<char> TagClose => "/>".AsSpan();
	private static ReadOnlySpan<char> Script => "script".AsSpan();

	private int _position = 0;

	private readonly LoopGuard LoopGuard = new(10000000);

	// Track line and column for error reporting
	private readonly int _currentLine = 1;
	private readonly int _currentColumn = 1;

	// Public accessors for error context
	public int CurrentLine => _currentLine;
	public int CurrentColumn => _currentColumn;
	public int CurrentPosition => _position;

	// Helper to get a snippet of HTML around the current position
	public string GetCurrentContext(int before = 20, int after = 20)
	{
		int start = System.Math.Max(0, _position - before);
		int end = System.Math.Min(_html.Length, _position + after);
		string snippet = HtmlSpan[start..end].ToString();

		// Add a marker (^) at the error position within the snippet
		int markerPos = _position - start;
		return snippet.Insert(markerPos, "❯❯❯").Insert(markerPos + 3, "❮❮❮");
	}

	// Update line/column when advancing
	public Either<Element, VoidElement> Parse()
	{
		SkipWhitespace();
		return ParseElement(null);//start with a null element
	}

	private Either<Element, VoidElement> ParseElement(Element? parent) //a void element cannot be a parent
	{

		//add clause to shortcircuit if we have reached the end when recursing back in prior to running all the attribute logic
		if (parent is not null && Match(parent.TagEnd))
		{
			return parent;
		}

		if (Match(CommentOpen))
		{
			SkipComment();
			SkipWhitespace();
			return ParseElement(parent); // Recursively parse next element after the comment
		}

		GuardAgainstNotTagOpen();
		Advance();

		var tagName = ParseTagName();
		if (IsDocType(tagName))
		{
			AdvanceToStartOfNextElement();
			GuardAgainstNotTagOpen();
			Advance(); // Skip '<'
			tagName = ParseTagName();
		}

		Either<Element, VoidElement> element = ElementFactory.Create(tagName);

		// Parse attributes
		MatchAndParseAttribute(element);

		// Handle self-closing tags
		if (Match(TagClose))
		{
			Advance(2); // Skip "/>"
			parent?.Add(element);
			return element;
		}

		GuardAgainstNotTagEnd();
		Advance(); // Skip '>'

		// Check if the element is a VoidElement and return immediately
		if (element.IsVoidElement())
		{
			//void elements can't have any children, but they may have a closing tag  even
			//though its not valid....we will handle it just in case
			parent?.Add(element);
			if (Match(ClosingTag(tagName)))
				Advance(ClosingTag(tagName).Length); // Skip closing tag
			return element;
		}

		var currentElement = element.AsElement();
		parent?.Add(currentElement);

		if (currentElement is not GooseHtml.Script)
		{
			// Parse inner text and children for non-void elements
			while (DoesntMatchEndOfElementTag(tagName))
			{
				if (MatchesStartOfNewElement()) //ignore malformed tags '</' that don't match the closing tag
				{
					ParseElement(parent: currentElement);
				}
				else
				{
					AddTextToElement(tagName, currentElement);
				}
			}

			Advance(ClosingTag(tagName).Length); // Skip closing tag
		}
		else
		{
			// Parse raw text content until </script>
			HandleScript(currentElement);
		}

		return currentElement;
	}

	private bool MatchesStartOfNewElement()
	{
		return Match(TagOpen) && !Match(CommentOpen) && !Match(MalformedEndTag);
	}

	private static bool IsDocType(ReadOnlySpan<char> tagName)
	{
		return tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase);
	}

	private bool DoesntMatchEndOfElementTag(ReadOnlySpan<char> tagName)
	{
		return !Match(ClosingTag(tagName)) && IsBeforeLastChar() && LoopGuard.ShouldContinue("parse sub elements");
	}

	private void AddTextToElement(ReadOnlySpan<char> tagName, Element currentElement)
	{
		var textContent = ParseText(ClosingTag(tagName)).Trim();
		if (textContent.Length > 0)
		{
			var text = textContent.RemoveComments();
			if (text.Length > 0)
				currentElement.Add(new TextElement(text.ToString()));
		}
	}

	private void GuardAgainstNotTagOpen()
	{
		if (!Match(TagOpen)) throw new Exception($"Expected '{TagOpen}' got '{CurrentChar()}'.  {GetCurrentContext()}");
	}

	private void GuardAgainstNotTagEnd()
	{
		if (!Match(TagEnd)) throw new Exception($"Expected '{TagEnd}'");
	}

	private void MatchAndParseAttribute(Either<Element, VoidElement> element)
	{
		while (!Match(TagEnd) && !Match(TagClose) && IsBeforeLastChar() && LoopGuard.ShouldContinue($"attributes"))
		{
			SkipWhitespace();
			var attr = ParseAttribute();
			if (attr is not null)
				element.Match(e => e.Add(attr), v => v.Add(attr));
		}
	}

	private void HandleScript(Element currentElement)
	{
		ReadOnlySpan<char> closingTag = ClosingTag(Script);
		int contentStart = _position;
		int closingIndex = _html.IndexOf(closingTag.ToString(), _position, StringComparison.Ordinal);

		if (closingIndex == -1)
		{
			throw new Exception($"Unclosed script element at line {GetCurrentContext()}");
		}

		string content = HtmlSpan[contentStart..closingIndex].ToString();
		currentElement.Add(new TextElement(content, false));
		_position = closingIndex + closingTag.Length;
	}

	private void AdvanceToStartOfNextElement()
	{
		while (!Match(TagEnd))
		{
			Advance();
		}
		Advance(); // Skip '>' after doctype
		SkipWhitespace();
	}

	private static ReadOnlySpan<char> ClosingTag(ReadOnlySpan<char> tagName) => $"</{tagName}>";


	private ReadOnlySpan<char> ParseTagName()
	{
		int start = _position;
		while (IsBeforeLastChar() && CurrentChar().IsValidTagNameChar())
		{
			Advance();
		}

		//return HtmlSpan[start.._position];
		return HtmlSpan[start.._position];

	}

	private Attributes.Attribute ParseAttribute()
	{
		SkipWhitespace();

		// Parse attribute name
		ReadOnlySpan<char> key = ParseAttributeKey();
		SkipWhitespace();

		// Check if the attribute has a value (e.g., "key" vs. "key=value")
		if (!Match(Equal))
		{
			// Boolean attribute (e.g., "checked" in <input checked>)
			return new EmptyAttribute(key.ToString());
		}

		Advance(); // Skip '='
		SkipWhitespace();

		// Parse the value (quoted, unquoted, or empty)
		var value = ParseAttributeValue();
		return AttributeFactory.Create(key.ToString(), value.ToString());
	}

	private ReadOnlySpan<char> ParseAttributeKey()
	{
		int start = _position;
		while (IsBeforeLastChar() &&
				!char.IsWhiteSpace(CurrentChar()) &&
				!Match(Equal) &&
				!Match(TagEnd) &&
				LoopGuard.ShouldContinue("parse attribute name"))
		{
			Advance();
		}

		var key = HtmlSpan[start.._position];
		return key;
	}

	private ReadOnlySpan<char> ParseAttributeValue()
	{

		if (_position >= Length)
		{
			throw new Exception($"Unexpected end of input while parsing attribute value at line {CurrentLine}, column {CurrentColumn}.\nContext: {GetCurrentContext()}");
		}


		int start = _position;
		//handle quoted values
		if (IsBeforeLastChar() && (Match(DoubleQuote) || Match(SingleQuote)))
		{
			char quote = CurrentChar();
			var indexOfNextQuote = _html.IndexOf(quote, _position + 1);

			if (indexOfNextQuote == -1)
			{
				throw new Exception($"Missing closing quote for attribute value at line {CurrentLine}, column {CurrentColumn}.\nContext: {GetCurrentContext()}");
			}

			var value = HtmlSpan[(start + 1)..indexOfNextQuote];
			Advance(indexOfNextQuote - _position + 1); // Skip closing quote
			return value;
		}
		//handle unquoted attribute values
		else
		{

			while (IsBeforeLastChar() &&
					!char.IsWhiteSpace(CurrentChar()) &&
					!Match(TagEnd) &&
					!Match(Equal) &&
					!Match(BackTick) &&
					!Match(DoubleQuote) && LoopGuard.ShouldContinue("parse end of attribute values"))
			{
				Advance();
			}

			return HtmlSpan[start.._position].Trim();
		}
	}

	private char CurrentChar()
	{
		return _html[_position];
	}

	private ReadOnlySpan<char> ParseText(ReadOnlySpan<char> tagName)
	{
		int start = _position;
		while (IsBeforeLastChar() && LoopGuard.ShouldContinue("parse text"))
		{
			IgnoreMalformedEndTag(tagName);
			if (Match(CommentOpen))
			{
				SkipComment();
			}
			else if (Match(TagOpen))
			{
				break;
			}
			else
			{
				Advance();
			}
		}
		return HtmlSpan[start.._position];
	}


	private void IgnoreMalformedEndTag(ReadOnlySpan<char> currentTagName)
	{
		if (Match(MalformedEndTag) && !Match(currentTagName))
		{
			Advance(2);
		}
	}

	private void SkipWhitespace()
	{
		while (IsBeforeLastChar() && char.IsWhiteSpace(CurrentChar()) && LoopGuard.ShouldContinue("skip whitespace"))
		{
			Advance();
		}
	}

	private bool IsBeforeLastChar() => _position < Length;

	private void SkipComment()
	{
		if (!Match(CommentOpen)) return;


		// Skip past "<!--"
		Advance(4);

		// Find the closing "-->"
		while (_position < Length - 2 && LoopGuard.ShouldContinue("skip comments"))
		{
			if (Match(CommentClose))
			{
				Advance(3);
				return;
			}
			Advance(1); // Skip "-->"
		}

		// If we reach here, the comment is unclosed (malformed HTML)
		throw new Exception("Unclosed HTML comment detected.");
	}

	private bool Match(ReadOnlySpan<char> text)
	{
		if (_position + text.Length > Length)
		{
			return false;
		}

		// Compare the spans
		return HtmlSpan.Slice(_position, text.Length).SequenceEqual(text);
	}
	private bool Match(char ch) => IsBeforeLastChar() && CurrentChar() == ch;

	private bool Match(string str)
	{
		return Match(str.AsSpan());
	}

	private void Advance(int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			if (_position >= _html.Length) break;
			_position++;
		}
	}

}
