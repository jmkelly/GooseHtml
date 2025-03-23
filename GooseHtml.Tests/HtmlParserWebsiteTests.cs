using Shouldly;

namespace GooseHtml.Tests;
public class HtmlParserWebTest
{
	private static readonly HttpClient _httpClient = new();

	private static readonly List<string> _websites =
    [
		// News & Media
		"https://www.wikipedia.org/",
			"https://www.cnn.com/",
			"https://www.bbc.com/news",
			"https://www.theverge.com/",
			"https://www.wired.com/",
			"https://www.nytimes.com/",
			"https://news.google.com/",
			"https://www.aljazeera.com/",
			//"https://www.reuters.com/", --this was forbidden
			"https://www.theguardian.com/international",

			// E-Commerce & Product Pages
			"https://www.amazon.com/",
			"https://www.ebay.com/",
			"https://www.walmart.com/",
			"https://www.bestbuy.com/",
			"https://www.aliexpress.com/",
			"https://www.etsy.com/",
			"https://www.newegg.com/",
			"https://www.costco.com/",
			"https://www.target.com/",
			"https://www.ikea.com/",

			// Social Media
			"https://twitter.com/",
			"https://www.instagram.com/",
			"https://www.facebook.com/",
			"https://www.tiktok.com/",
			"https://www.linkedin.com/",
			"https://www.reddit.com/",
			"https://www.pinterest.com/",
			"https://www.snapchat.com/",
			"https://www.youtube.com/",
			"https://www.twitch.tv/",

			// Developer & Tech Documentation
			"https://developer.mozilla.org/",
			"https://learn.microsoft.com/",
			"https://docs.python.org/3/",
			"https://stackoverflow.com/",
			"https://github.com/",
			"https://docs.docker.com/",
			"https://kubernetes.io/docs/",
			"https://openai.com/",
			"https://www.kernel.org/",
			"https://wiki.archlinux.org/",

			// Educational & Research
			"https://www.khanacademy.org/",
			"https://www.coursera.org/",
			"https://www.harvard.edu/",
			"https://www.mit.edu/",
			"https://www.nasa.gov/",
			"https://www.stanford.edu/",
			"https://www.ted.com/",
			"https://www.springer.com/",
			"https://arxiv.org/",
			"https://pubmed.ncbi.nlm.nih.gov/",

			// Miscellaneous
			"https://www.imdb.com/",
			"https://www.rottentomatoes.com/",
			"https://www.chess.com/",
			"https://www.boredpanda.com/",
			"https://weather.com/",
			"https://www.nationalgeographic.com/",
			"https://www.espn.com/",
			"https://www.fifa.com/",
			"https://www.tripadvisor.com/",
			"https://www.airbnb.com/"
	];

	[Fact]
	[Trait("Category", "WebFetch")]
	public async Task Should_Parse_Multiple_Websites()
	{
		Directory.CreateDirectory("test_pages"); // Ensure directory exists
		string currentMonth = DateTime.UtcNow.ToString("yyyy-MM");

		foreach (var url in _websites)
		{
			Console.WriteLine($"Parsing: {url}");

			string sanitizedFilename = $"test_pages/{SanitizeUrl(url)}_{currentMonth}.html";
			string html;
			if (File.Exists(sanitizedFilename))
				html = await File.ReadAllTextAsync(sanitizedFilename);
			else {
				html = await FetchHtmlFromUrl(url);
				// Save for debugging
				await File.WriteAllTextAsync(sanitizedFilename, html);
			}

			// Parse
			var parser = new HtmlParser(html); // Ensure you have an HtmlParser class
			var page = parser.Parse();

			page.ShouldNotBeNull($"Parsing failed for {url}");
		}
	}

	private static async Task<string> FetchHtmlFromUrl(string url)
	{
		_httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
		var response = await _httpClient.GetAsync(url);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadAsStringAsync();
	}

	private static string SanitizeUrl(string url)
	{
		return url.Replace("https://", "").Replace("http://", "").Replace("/", "_");
	}
}

