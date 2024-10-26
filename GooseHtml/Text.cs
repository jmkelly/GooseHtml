namespace GooseHtml;

public record Text(string Value, bool EscapeHtml = true)
{
    public override string? ToString()
    {
        return EscapeHtml ? Value.EscapeHtml() : Value;
    }
}


public static class TextExtensions
{
	public static string EscapeHtml(this string value)
	{
		return System.Net.WebUtility.HtmlEncode(value);
	}
}

