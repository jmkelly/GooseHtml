using System.Text;

namespace GooseHtml;


//TODO:
//keep track of line numbers and position within line for better error messages

public class HtmlParser(string html)
{
	private readonly string _html = html;
	private int _position = 0;
	private int _lineNumber = 1;
	private int _columnNumber = 1;

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
		if (tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase))
        {
            AdvanceToStartOfNextElement();
            if (!Match('<')) return null;
            Advance(); // Skip '<'
            tagName = ParseTagName();
        }

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

    private void AdvanceToStartOfNextElement()
    {
        while (!Match('>'))
        {
            Advance();
        }
        //we want to skip the doctype, so start at the next element
        Advance();
        SkipWhitespace();
    }

    private string ParseTagName()
	{
		int start = _position;
		while (_position < _html.Length && _html[_position].IsValidChar())
		{
			_position++;
		}

		if (start == _position)
		{
			//get the characters before and after current from last whitepsace to next whitespace from current position
			throw new Exception($"Invalid tag name at line:{_lineNumber}:{_columnNumber} - {ShowCharInString(GetCurrentWord(), _html[_position])}");
		}

		return _html.AsSpan(start, _position - start).ToString(); // ✅ Uses Span
	}

	private static string ShowCharInString(string text, char chr)
	{
		//show the char in the string surrounded by * on both sides
		return $"{text.Substring(0, text.IndexOf(chr))}**{text.Substring(text.IndexOf(chr))}";
		//return string.Concat(text.AsSpan(0, text.IndexOf(chr)), "*", text.AsSpan(text.IndexOf(chr)));
	}

	private string GetCurrentWord()
	{
		/*int backwards = _position;*/
		/*int forwards = _position;*/
		/*while (backwards < _html.Length && _html[backwards].IsValidChar() && _html[backwards] != ' ')*/
		/*{*/
		/*	backwards--;*/
		/*}*/
		/*while (forwards < _html.Length && _html[forwards].IsValidChar() && _html[forwards] != ' ')*/
		/*{*/
		/*	forwards++;*/
		/*}*/
		int start = _position - 20;
		int end = _position + 10;
		return _html[start..end]; 
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
			return new Attributes.Attribute(key);

		Advance(); // Skip '='
		SkipWhitespace();

		string value = ParseAttributeValue().ToString();
		return AttributeFactory.Create(key, value);
	}

	private ReadOnlySpan<char> ParseAttributeValue()
	{
		if (!Match('"') && !Match('\''))
			throw new Exception($"Expected quote for attribute value at {_lineNumber}:{_columnNumber}");

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
		{
			if (_html[_position] == '\n')
			{
				_lineNumber++;
				_columnNumber = 1;
			}
			_position++;

		}
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
		_columnNumber += count;
	}

}
public static class HtmlParserExtensions
{
	public static bool IsValidChar(this char ch) => char.IsLetterOrDigit(ch) || ch == '!';
}

