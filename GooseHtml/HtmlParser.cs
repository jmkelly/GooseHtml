using GooseHtml.Attributes;

namespace GooseHtml;

public class HtmlParser(string html) : IParser
{
	private readonly string _html = html;
    private ReadOnlySpan<char> HtmlSpan => _html.AsSpan();
	private static ReadOnlySpan<char> CommentOpen => "<!--".AsSpan();
	private static ReadOnlySpan<char> CommentClose => "-->".AsSpan();
    private int _position = 0;

	private readonly ThreadSafeLoopGuard LoopGuard = new(10000000);

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
		string snippet = _html[start..end];

		// Add a marker (^) at the error position within the snippet
		int markerPos = _position - start;
		return snippet.Insert(markerPos, "❯❯❯").Insert(markerPos + 3, "❮❮❮");
	}

	// Update line/column when advancing
	public OneOf<Element, VoidElement> Parse()
	{
		SkipWhitespace();
		return ParseElement(null);//start with a null element
	}

	private OneOf<Element, VoidElement> ParseElement(Element? parent) //a void element cannot be a parent
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

		if (!Match('<')) throw new Exception($"Expected '<' got '{CurrentChar()}'.  {GetCurrentContext()}");
		Advance();

		var tagName = ParseTagName();
		if (tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase))
		{
			AdvanceToStartOfNextElement();
			if (!Match('<')) throw new Exception($"Expected '<' got '{CurrentChar()}'.  {GetCurrentContext()}");
			Advance(); // Skip '<'
			tagName = ParseTagName();
		}

		OneOf<Element, VoidElement> element = ElementFactory.Create(tagName);

		// Parse attributes
		while (!Match('>') && !Match("/>") && IsBeforeLastChar() && LoopGuard.ShouldContinue($"attributes {GetCurrentContext()}"))
		{
			SkipWhitespace();
			var attr = ParseAttribute();
			if (attr is not null)
				element.Match(e => e.Add(attr), v => v.Add(attr));
		}

		// Handle self-closing tags
		if (Match("/>"))
		{
			Advance(2); // Skip "/>"
			parent?.Add(element);
			return element;
		}

		if (!Match('>')) throw new Exception("Expected '>' or '/>'");
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

		if (currentElement is not Script)
		{
			// Parse inner text and children for non-void elements
			while (!Match(ClosingTag(tagName)) && IsBeforeLastChar() && LoopGuard.ShouldContinue("parse sub elements"))
			{
				if (Match('<') && !Match("<!--") && !Match("</")) //ignore malformed tags that don't match the closing tag
				{
					ParseElement(parent: currentElement);
				}
				else
				{
					var textContent = ParseText(ClosingTag(tagName)).Trim();
					if (textContent.Length > 0)
					{
						var text = textContent.RemoveComments();
						if (text.Length > 0)
							currentElement.Add(new TextElement(text.ToString()));
					}
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

	private void HandleScript(Element currentElement)
	{
		ReadOnlySpan<char> closingTag = ClosingTag("script");
			//"</script>");
		int contentStart = _position;
		bool found = false;

		while (_position <= _html.Length - closingTag.Length && LoopGuard.ShouldContinue("handle script"))
		{
			if (Match(closingTag))
			{
				found = true;
				break;
			}
			Advance();
		}

		if (!found)
			throw new Exception($"Unclosed script element at line {CurrentLine}, column {CurrentColumn}.");

		string content = HtmlSpan[contentStart.._position].ToString();
		currentElement.Add(new TextElement(content, false));
		Advance(closingTag.Length);
	}

	private void AdvanceToStartOfNextElement()
	{
		while (!Match('>')) 
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
        int start = _position;
        while (IsBeforeLastChar() &&
                !char.IsWhiteSpace(CurrentChar()) &&
                !Match('=') &&
                !Match('>') &&
                LoopGuard.ShouldContinue("parse attribute name"))
        {
            Advance();
        }

		var key = HtmlSpan[start.._position];
        SkipWhitespace();

        // Check if the attribute has a value (e.g., "key" vs. "key=value")
        if (!Match('='))
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

    private ReadOnlySpan<char> ParseAttributeValue()
	{

		if (_position >= _html.Length)
		{
			throw new Exception($"Unexpected end of input while parsing attribute value at line {CurrentLine}, column {CurrentColumn}.\nContext: {GetCurrentContext()}");
		}


		int start = _position;
		//handle quoted values

		if (IsBeforeLastChar() && (Match('"') || Match('\'')))
        {
            char quote = CurrentChar();
            Advance(); // Skip opening quote
            start = _position;

            while (IsBeforeLastChar() &&
                    CurrentChar() != quote &&
                    LoopGuard.ShouldContinue("parse attribute value"))
                Advance();

            var value = HtmlSpan[start.._position];
            Advance(); // Skip closing quote
            return value;
        }

        while (IsBeforeLastChar() && 
				!char.IsWhiteSpace(CurrentChar()) && 
				!Match('>') && 
				!Match('=') && 
				!Match('`') && 
				!Match('"') && LoopGuard.ShouldContinue("parse end of attribute values"))
		{
			Advance();
		}

		return HtmlSpan[start.._position].Trim();
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
			if (Match("<!--"))
			{
				SkipComment();
			}
			else if (Match('<')) 
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
		if (Match("</") && !Match(currentTagName))
		{
			Advance(2);
		}
	}

    private static bool IsValid(char character)
	{
		// Check for acceptable characters: letters, digits, whitespace, and some punctuation
		return char.IsLetterOrDigit(character) || 
			char.IsWhiteSpace(character) || 
			character == '-' || 
			character == '_' || 
			character == '.' || 
			character == ',' || 
			character == '!' || 
			character == '?' || 
			character == ':' || 
			character == ';' || 
			character == '\'' || 
			character == '\"' || 
			character == '/' || 
			character == '\\' || 
			character == '@' || 
			character == '<' || 
			character == '>' || 
			character == '=' || 
			character == '#';
	}

	private void SkipWhitespace()
	{
		while (IsBeforeLastChar() && (char.IsWhiteSpace(_html[_position]) ||  !IsValid(_html[_position])) && LoopGuard.ShouldContinue("skip whitespace"))
		{
			Advance();
		}
	}

	private bool IsBeforeLastChar() => _position < _html.Length;

	private void SkipComment()
	{
		if (!Match(CommentOpen)) return;


		// Skip past "<!--"
		Advance(4);

		// Find the closing "-->"
		while (_position < HtmlSpan.Length - 2 && LoopGuard.ShouldContinue("skip comments"))
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

	private bool Match(ReadOnlySpan<char> strSpan) 
	{
		if (_position + strSpan.Length > HtmlSpan.Length)
		{
			return false;
		}

		// Compare the spans
		return HtmlSpan.Slice(_position, strSpan.Length).SequenceEqual(strSpan);
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
