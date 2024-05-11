
using Shouldly;

namespace HtmlBuilder.Tests;


public class HtmlParserTests
{
	const string TestPagesDirectory = "TestPages";
	[Fact(Skip="used to generate html files")]
	//[Fact]
	public async Task HtmlParser_ShouldGetTheGoogleSearchPageAsync()
	{
		var reader = new HtmlReader();
		var html = await reader.ReadAsync("https://google.com.au");
		html.ShouldNotBeNullOrWhiteSpace();

		var writer = new HtmlWriter();
		await writer.WriteAsync($"{TestPagesDirectory}/google.html", html);
	}


	[Fact]
	public async Task HtmlParser_ShouldParseGoogleHtmlFile()
	{
		var reader = new HtmlReader();
		var html = await reader.ReadAsync(filePath: $"{TestPagesDirectory}/google.html");
		var parser = new HtmlParser();
		var parsed = parser.Parse(html);

		parsed.Head().ShouldBeOfType<Head>();
		parsed.Body().ShouldBeOfType<Body>();
	}
}
