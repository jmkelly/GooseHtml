using System.Text.Encodings.Web;

namespace GooseHtml;

public record Text(string Value)
{
    public override string? ToString()
    {
		return Value.EscapeHtml();
    }
}


public static class TextExtensions
{
	public static string EscapeHtml(this string value)
	{
		return HtmlEncoder.Create().Encode(value);
	}
}

