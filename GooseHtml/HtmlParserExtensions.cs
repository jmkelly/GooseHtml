namespace GooseHtml;

public static class HtmlParserExtensions
{
	public static bool IsValidChar(this char ch) => char.IsLetterOrDigit(ch) || ch == '!';
    public static bool IsValidTagNameChar(this char ch) => char.IsLetterOrDigit(ch) || ch == '!' || ch == '-';
}

