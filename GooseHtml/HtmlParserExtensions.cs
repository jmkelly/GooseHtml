using System.Text.RegularExpressions;

namespace GooseHtml;

public static class HtmlParserExtensions
{
	public static bool IsValidChar(this char ch) => char.IsLetterOrDigit(ch) || ch == '!';
    public static bool IsValidTagNameChar(this char ch) => char.IsLetterOrDigit(ch) || ch == '!' || ch == '-';
    //public static bool IsValidCharLlm(this char ch) => char.IsLetterOrDigit(ch);

    public static bool IsValidTagNameCharLlm(this char c)
    {
        return char.IsLetterOrDigit(c) || c == '-';
    }

    /*public static string RemoveComments(this string input)*/
    /*{*/
    /*    // Implementation for removing HTML comments*/
    /*    return MyRegex().Replace(input, string.Empty);*/
    /*}*/

    public static bool IsDoctype(string tagName) => 
        tagName.Equals("!doctype", StringComparison.InvariantCultureIgnoreCase);
    //[GeneratedRegex("<!--.*?-->", RegexOptions.Singleline)]
    //private static partial Regex MyRegex();
}

