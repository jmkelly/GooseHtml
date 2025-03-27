namespace GooseHtml.Tests;

public static class TestExtensions
{
	public static string SanitizeUrl(this string url)
	{
		return url.Replace("https://", "").Replace("http://", "").Replace("/", "_");
	}
}


