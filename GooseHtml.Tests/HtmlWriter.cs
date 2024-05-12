namespace GooseHtml.Tests;

public class HtmlWriter
{

	public async Task WriteAsync(string filePath, string html)
	{
		await File.WriteAllTextAsync(filePath, html);
	}
}

