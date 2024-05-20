//using System.Text.Encodings.Web;

namespace GooseHtml;

public record Text(string Value, bool EscapeHtml = true)
{
    public override string? ToString()
    {
		if (EscapeHtml)
			return Value.EscapeHtml();
		else
			return Value;
    }
}


public static class TextExtensions
{
	public static string EscapeHtml(this string value)
	{
		return System.Net.WebUtility.HtmlEncode(value);
		//return HtmlEncoder.Create().Encode(value);
	}
}

