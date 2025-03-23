using System.Text;
using GooseHtml.Attributes;

namespace GooseHtml;

public class HtmlParser(string html)
{
	private readonly string _html = html;
	private int _position = 0;
	private int _lineNumber = 1;
	private int _columnNumber = 1;

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
		Advance(); // Skip '<'

		string tagName = ParseTagName();
		if (tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase))
		{
			AdvanceToStartOfNextElement();
			if (!Match('<')) throw new Exception($"Expected '<' got '{_html[_position]}'.  {GetCurrentContext()}");
			Advance(); // Skip '<'
			tagName = ParseTagName();
		}

		OneOf<Element, VoidElement> element = ElementFactory.Create(tagName);

		// Parse attributes
		while (!Match('>') && !Match("/>") && _position < _html.Length)
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
			parent?.Add(element);
			return element;
		}

		var currentElement = element.AsElement();
		parent?.Add(currentElement);

        if (currentElement is not Script)
        {
            // Parse inner text and children for non-void elements
            while (!Match($"</{tagName}>"))
            {
                if (Match('<'))
                {
                    ParseElement(parent: currentElement);
                }
                else
                {
                    var textContent = ParseText().ToString().Trim();
                    if (!string.IsNullOrEmpty(textContent))
                    {
                        currentElement.Add(new Text(textContent));
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

        while (_position <= _html.Length - closingTag.Length)
        {
            if (Match(closingTag))
            {
                string content = _html[contentStart.._position];
                if (!string.IsNullOrWhiteSpace(content))
                {
                    currentElement.Add(new TextElement(content, false));
                }
                Advance(closingTag.Length); // Skip closing tag
                break;
            }
            Advance();
        }
    }

    private void AdvanceToStartOfNextElement()
	{
		while (!Match('>')) Advance();
		Advance(); // Skip '>' after doctype
		SkipWhitespace();
	}

	private string ParseTagName()
	{
		int start = _position;
		while (_position < _html.Length && _html[_position].IsValidTagNameChar())
		{
			_position++;
		}

		if (start == _position)
		{
			throw new Exception($"invalid tag name at line {_lineNumber}, column {_columnNumber}.\nContext: {GetCurrentContext()}");
		}

		return _html[start.._position];
	}

	private Attributes.Attribute ParseAttribute()
	{
		SkipWhitespace();

		// Parse attribute name
		int start = _position;
		while (_position < _html.Length && 
			   !char.IsWhiteSpace(_html[_position]) && 
			   _html[_position] != '=' && 
			   _html[_position] != '>' && 
			   _html[_position] != '/')
		{
			_position++;
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

		if (_position <= _html.Length && (Match('"') || Match('\''))) 
		{
			char quote = _html[_position];
			Advance(); // Skip opening quote
			start = _position;

			while (_position < _html.Length && _html[_position] != quote)
				_position++;

			string value = _html[start.._position];
			Advance(); // Skip closing quote
			return value;
		}

		while (_position < _html.Length && 
			   !char.IsWhiteSpace(_html[_position]) && 
			   _html[_position] != '>' && 
			   _html[_position] != '=' && 
			   _html[_position] != '`' && 
			   _html[_position] != '"')
			   
		{
			Advance();
		}

    string unquotedValue = _html[start.._position];
	return unquotedValue.Trim();
	}

	private ReadOnlySpan<char> ParseText()
	{
		int start = _position;
		while (_position < _html.Length && _html[_position] != '<')
			_position++;

		return _html.AsSpan(start, _position - start);
	}

	private void SkipWhitespace()
	{
		while (_position < _html.Length && char.IsWhiteSpace(_html[_position]))
		{
			if (_html[_position] == '\n')
			{
				_lineNumber++;
				_columnNumber = 1;
			}
			_position++;
		}
	}

	private void SkipComment()
	{
		if (!Match("<!--")) return;


		// Skip past "<!--"
		Advance(4);

		// Find the closing "-->"
		while (_position < _html.Length - 2)
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
	private bool Match(string str) => _position + str.Length <= _html.Length && _html.Substring(_position, str.Length) == str;

    private void Advance(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            if (_position >= _html.Length) break;

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
