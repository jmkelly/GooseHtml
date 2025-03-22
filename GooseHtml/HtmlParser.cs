using System.Text;

namespace GooseHtml;

public class HtmlParser(string html)
{
    private readonly string _html = html;
    private int _position = 0;
    private int _lineNumber = 1;
    private int _columnNumber = 1;

    public OneOf<Element, VoidElement> Parse()
    {
        SkipWhitespace();
        return ParseElement();
    }

    private static void Write(string text) => Console.WriteLine(text);

    private OneOf<Element, VoidElement> ParseElement()
    {
        if (!Match('<')) throw new Exception("Expected '<' got '" + _html[_position] + "'");
        Advance(); // Skip '<'

        string tagName = ParseTagName();
        Write("parsed tag name: " + tagName);
        if (tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase))
        {
            AdvanceToStartOfNextElement();
            if (!Match('<')) throw new Exception("Expected '<' got '" + _html[_position] + "'");
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
            Write("end of empty tag name: " + tagName);
            Advance(2); // Skip "/>"
            return element;
        }

        if (!Match('>')) throw new Exception("Expected '>' or '/>'");
        Advance(); // Skip '>'

        // Check if the element is a VoidElement (T1) and return immediately
        if (element.IsT1)
        {
            return element;
        }

        // Parse inner text and children for non-void elements
        StringBuilder innerTextBuilder = new();
        int safetyCounter = 0;
        const int maxIterations = 10000;

        while (!Match($"</{tagName}>") && safetyCounter++ < maxIterations)
        {
            if (Match('<'))
            {
                var childElement = ParseElement();
                element.AsT0.Add(childElement);
            }
            else
            {
                var textContent = ParseText().ToString();
                if (!string.IsNullOrEmpty(textContent))
                {
                    element.AsT0.Add(new Text(textContent));
                }
            }
        }

        if (safetyCounter >= maxIterations)
        {
            throw new Exception($"{maxIterations} iterations exceeded. Closing tag for {tagName} not found.");
        }

        Advance($"</{tagName}>".Length); // Skip closing tag
        return element;
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
            throw new Exception($"Invalid tag name at {_lineNumber}:{_columnNumber}");
        }

        return _html.Substring(start, _position - start);
    }

    private Attributes.Attribute ParseAttribute()
    {
        SkipWhitespace();
        if (_position >= _html.Length) return null;

        int start = _position;
        while (_position < _html.Length && (char.IsLetterOrDigit(_html[_position]) || _html[_position] == '-'))
            _position++;

        string key = _html.Substring(start, _position - start);
        SkipWhitespace();

        if (!Match('=')) return new Attributes.Attribute(key);
        Advance(); // Skip '='

        SkipWhitespace();
        var value = ParseAttributeValue();
        return new Attributes.Attribute(key, value);
    }

    private string ParseAttributeValue()
    {
        if (_position >= _html.Length || (!Match('"') && !Match('\''))) 
            throw new Exception("Expected attribute value quote.");

        char quote = _html[_position];
        Advance(); // Skip opening quote

        int start = _position;
        while (_position < _html.Length && _html[_position] != quote)
            _position++;

        string value = _html.Substring(start, _position - start);
        Advance(); // Skip closing quote
        return value;
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

    private bool Match(char ch) => _position < _html.Length && _html[_position] == ch;
    private bool Match(string str) => _position + str.Length <= _html.Length && _html.Substring(_position, str.Length) == str;

    private void Advance(int count = 1)
    {
        _position += count;
        _columnNumber += count;
    }
}
