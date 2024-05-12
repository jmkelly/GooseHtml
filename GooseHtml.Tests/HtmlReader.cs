namespace GooseHtml.Tests;

public class HtmlReader 
{
	public async Task<string> ReadAsync(string filePath)
	{
		string htmlContent = await File.ReadAllTextAsync(filePath);
		return htmlContent;
	}

	public async Task<string> ReadAsync(Uri uri)
	{
        using (HttpClient client = new HttpClient())
        {
			HttpResponseMessage response = await client.GetAsync(uri);
			response.EnsureSuccessStatusCode(); 			
			string htmlContent = await response.Content.ReadAsStringAsync();
			return htmlContent;
        }
	}
}

