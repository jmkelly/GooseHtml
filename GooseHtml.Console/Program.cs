﻿using GooseHtml;

HttpClient httpClient = new();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Parsing websites");

List<string> websites =
[
	// News & Media
	"https://www.wikipedia.org/",
	"https://www.cnn.com/",
	"https://www.bbc.com/news",
	"https://www.theverge.com/",
	//"https://www.wired.com/", --forbidden
	"https://www.nytimes.com/",
	"https://news.google.com/",
	"https://www.aljazeera.com/",
	//"https://www.reuters.com/", --this was forbidden
	"https://www.theguardian.com/international",

	// E-Commerce & Product Pages
	"https://www.amazon.com/",
	"https://www.ebay.com/",
	//"https://www.walmart.com/", //--timeout
	//"https://www.bestbuy.com/", //--timeout
	//"https://www.aliexpress.com/",
	//"https://www.etsy.com/",
	"https://www.newegg.com/",
	//"https://www.costco.com/",//timeout
	"https://www.target.com/",
	"https://www.ikea.com/",

	// Social Media
	//"https://twitter.com/", bad request
	"https://www.instagram.com/",
	"https://www.facebook.com/",
	"https://www.tiktok.com/",
	//"https://www.linkedin.com/", //error 99999
	"https://www.reddit.com/", //--timeout
	"https://www.pinterest.com/",
	"https://www.snapchat.com/",
	"https://www.youtube.com/",
	"https://www.twitch.tv/",

	// Developer & Tech Documentation
	"https://developer.mozilla.org/",
	"https://learn.microsoft.com/",
	"https://docs.python.org/3/",
	"https://stackoverflow.com/",
	"https://github.com/", //--timeout
	"https://docs.docker.com/",
	//"https://kubernetes.io/docs/", --bad request
	//"https://openai.com/", --forbidden
	"https://www.kernel.org/",
	"https://wiki.archlinux.org/",
	"https://www.w3schools.com/html/html_examples.asp",
	// Educational & Research
	"https://www.khanacademy.org/",
	"https://www.coursera.org/",
	"https://www.harvard.edu/", //timeout
	"https://www.mit.edu/", //skipped because likely timeout
	"https://www.nasa.gov/", //skipped because likely timeout
	"https://www.stanford.edu/",
	"https://www.ted.com/",
	"https://www.springer.com/",
	"https://arxiv.org/", //timeout
	"https://pubmed.ncbi.nlm.nih.gov/",

	// Miscellaneous
	"https://www.imdb.com/",
	"https://www.rottentomatoes.com/",
	"https://www.chess.com/",
	"https://www.boredpanda.com/",
	"https://weather.com/",
	"https://www.nationalgeographic.com/",
	"https://www.espn.com/",
	//"https://www.fifa.com/", --not found
	//"https://www.tripadvisor.com/", --forbidden
	"https://www.airbnb.com/"
	];

await ParseWebsites(websites);

async Task ParseWebsites(List<string> websites)
{
	Directory.CreateDirectory("test_pages"); // Ensure directory exists
	string currentMonth = DateTime.UtcNow.ToString("yyyy-MM");

	foreach (var url in websites)
	{
		Console.WriteLine($"Parsing: {url}");

		string sanitizedFilename = $"test_pages/{url.SanitizeUrl()}_{currentMonth}.html";
		string html;
		Console.WriteLine($"requesting {url}");
		if (File.Exists(sanitizedFilename))
			html = await File.ReadAllTextAsync(sanitizedFilename);
		else {
			html = await FetchHtmlFromUrl(httpClient,url);
			// Save for debugging
			await File.WriteAllTextAsync(sanitizedFilename, html);
		}

		// Parse
		Console.WriteLine($"requested {url} ok");
		var parser = new HtmlParser(html); // Ensure you have an HtmlParser class
		var page = parser.Parse();
		Console.WriteLine($"parsing complete for {url} ok");

	}
}

async Task<string> FetchHtmlFromUrl(HttpClient httpClient, string url)
{
	httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
	var response = await httpClient.GetAsync(url);
	response.EnsureSuccessStatusCode();
	return await response.Content.ReadAsStringAsync();
}
