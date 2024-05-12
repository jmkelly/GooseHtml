
using Shouldly;

namespace GooseHtml.Tests;


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
}
