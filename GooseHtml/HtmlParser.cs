using System.Text;
using GooseHtml.Attributes;

namespace GooseHtml;

public class HtmlParser(string html)
{
    private readonly string _html = html;
    private int _position = 0;

    public Element? Parse()
    {
        SkipWhitespace();
        return ParseElement();
    }

    private Element? ParseElement()
    {
        if (!Match('<')) return null;
        Advance(); // Skip '<'

        string tagName = ParseTagName();
        Element element = ElementFactory.Create(tagName);

        // Parse attributes
        while (!Match('>') && !Match("/>") && _position < _html.Length)
        {
            SkipWhitespace();
            var attr = ParseAttribute();
			if (attr is not null)
                element.Add(attr);
        }

        // Handle self-closing tags
        if (Match("/>"))
        {
            Advance(2);
            return element;
        }

        Advance(); // Skip '>'

        // Parse inner text and children
        StringBuilder innerTextBuilder = new();
		int safetyCounter = 0;
		const int maxIterations = 10000;

        while (!Match($"</{tagName}>") && safetyCounter++ < maxIterations)
        {
            if (Match('<'))
            {
                var el = ParseElement();
				if (el is not null)
					element.Add(el);
            }
            else
			{
                innerTextBuilder.Append(ParseText());
				element.Add(new Text(innerTextBuilder.ToString()));
			}

        }
		if (safetyCounter >= maxIterations)
		{
			throw new Exception($"{safetyCounter} iterations exceeded with no closing tag for {tagName} found. Cannot parse, please check supplied HTML.");
		}

        Advance($"</{tagName}>".Length);
        return element;
    }

	private string ParseTagName()
	{
		int start = _position;
		while (_position < _html.Length && char.IsLetterOrDigit(_html[_position]))
			_position++;

		if (start == _position)
			throw new Exception("Invalid tag name");

		return _html.AsSpan(start, _position - start).ToString(); // ✅ Uses Span
	}


    private Attributes.Attribute? ParseAttribute()
    {
        SkipWhitespace();

        int start = _position;
        while (_position < _html.Length && (char.IsLetterOrDigit(_html[_position]) || _html[_position] == '-'))
            _position++;

        if (start == _position) return null;
        string key = _html[start.._position];

        SkipWhitespace();
        if (!Match('='))
            return new Attributes.Attribute(key, string.Empty);

        Advance(); // Skip '='
        SkipWhitespace();

        string value = ParseAttributeValue().ToString();
        return AttributeFactory.Create(key, value);
    }

    private ReadOnlySpan<char> ParseAttributeValue()
    {
        if (!Match('"') && !Match('\''))
            throw new Exception("Expected quote for attribute value");

        char quote = _html[_position];
        Advance(); // Skip opening quote

        int start = _position;
        while (_position < _html.Length && _html[_position] != quote)
            _position++;

        //string value = _html[start.._position];
		var value = _html.AsSpan(start, _position - start);
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
            _position++;
    }

    private bool Match(char ch)
    {
        return _position < _html.Length && _html[_position] == ch;
    }

    private bool Match(string str)
    {
        return _html[_position..].StartsWith(str);
    }

    private void Advance(int count = 1)
    {
        _position += count;
    }

}
internal static class ElementFactory
{

    internal static  Element Create(string tagName)
    {
        return tagName.ToLower() switch
        {
            "b" => new B(),
			"body" => new Body(),
            "div" => new Div(),
			"footer" => new Footer(),
			"head" => new Head(),
			"html" => new Html(),
            "img" => new Img(),
            "p" => new P(),
            _ => throw new ArgumentException($"element type: {tagName} unknown")
        };
    }
}

internal static class AttributeFactory
{
	internal static GooseHtml.Attributes.Attribute Create(string key, string value)
	{
        return key.ToLower() switch
        {
            "accept" => new Accept(value),
			"accept-charset" => new AcceptCharset(value),
			"charset" => new Charset(value),
			"content" => new Content(value),
			"href" => new Href(value),
			"id" => new Id(value),
			"class" => new GooseHtml.Attributes.Class(value),
			"src" => new Src(value),

            _ => throw new ArgumentException($"Attribute {key} unknown"),
        };
    }
}

