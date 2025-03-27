using GooseHtml.Attributes;

namespace GooseHtml;

public class HtmlParser(string html) : IParser
{
	private readonly string _html = html;
    private ReadOnlySpan<char> HtmlSpan => _html.AsSpan();
    private int _position = 0;

	private readonly ThreadSafeLoopGuard LoopGuard = new(10000000);

	// Track line and column for error reporting
	private int _currentLine = 1;
	private int _currentColumn = 1;

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

		if (Match("<!--"))
		{
			SkipComment();
			SkipWhitespace();
			return ParseElement(parent); // Recursively parse next element after the comment
		}

		if (!Match('<')) throw new Exception($"Expected '<' got '{_html[_position]}'.  {GetCurrentContext()}");
		Advance();

		string tagName = ParseTagName().ToString();
		if (tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase))
		{
			AdvanceToStartOfNextElement();
			if (!Match('<')) throw new Exception($"Expected '<' got '{_html[_position]}'.  {GetCurrentContext()}");
			Advance(); // Skip '<'
			tagName = ParseTagName().ToString();
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
					var textContent = ParseText(ClosingTag(tagName)).ToString().Trim();
					if (!string.IsNullOrEmpty(textContent))
					{
						//todo: fix the text parser to remove comments rather than this workaround
						var text = textContent.RemoveComments();
						currentElement.Add(new Text(text));
					}
				}
			}

			Advance($"</{tagName}>".Length); // Skip closing tag
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
		string closingTag = "</script>";
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

		string content = _html[contentStart.._position];
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

	private static string ClosingTag(string tagName) => $"</{tagName}>";

	private ReadOnlySpan<char> ParseTagName()
	{
		int start = _position;
		while (IsBeforeLastChar() && _html[_position].IsValidTagNameChar())
		{
			_position++;
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
                !char.IsWhiteSpace(_html[_position]) &&
                !Match('=') &&
                !Match('>') &&
                LoopGuard.ShouldContinue("parse attribute name"))
        {
            Advance();
        }

        string key = _html[start.._position];
        SkipWhitespace();

        // Check if the attribute has a value (e.g., "key" vs. "key=value")
        if (!Match('='))
        {
            // Boolean attribute (e.g., "checked" in <input checked>)
            return new EmptyAttribute(key);
        }

        Advance(); // Skip '='
        SkipWhitespace();

        // Parse the value (quoted, unquoted, or empty)
        string value = ParseAttributeValue();
        return AttributeFactory.Create(key, value);
    }

    private string ParseAttributeValue()
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

            string value = _html[start.._position];
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

		string unquotedValue = _html[start.._position];
		return unquotedValue.Trim();


	}

    private char CurrentChar()
    {
        return _html[_position];
    }

    private ReadOnlySpan<char> ParseText(string tagName)
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
		return _html.AsSpan(start,_position - start);
	}


    private void IgnoreMalformedEndTag(string currentTagName)
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
		if (!Match("<!--")) return;


		// Skip past "<!--"
		Advance(4);

		// Find the closing "-->"
		while (_position < _html.Length - 2 && LoopGuard.ShouldContinue("skip comments"))
		{
			if (Match("-->"))
			{
				Advance(3);
				return;
			}
			Advance(1); // Skip "-->"
		}

		// If we reach here, the comment is unclosed (malformed HTML)
		throw new Exception("Unclosed HTML comment detected.");
	}

	private bool Match(char ch) => _position < _html.Length && _html[_position] == ch;

	private bool Match(string str) 
	{
		ReadOnlySpan<char> strSpan = str.AsSpan(); // Convert the string to a ReadOnlySpan<char>
		
		// Check if the position and length are within bounds
		if (_position + strSpan.Length > HtmlSpan.Length)
		{
			return false;
		}

		// Compare the spans
		return HtmlSpan.Slice(_position, strSpan.Length).SequenceEqual(strSpan);
	}

	private void Advance(int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			if (_position >= _html.Length) break;

			//keeping this is string because its faster than using the span....needs further investigation
			if (_html[_position] == '\n')
			{
				_currentLine++;
				_currentColumn = 1;
			}
			else
			{
				_currentColumn++;
			}

			_position++;
		}
	}

}
