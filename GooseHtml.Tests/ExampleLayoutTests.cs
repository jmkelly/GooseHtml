using Shouldly;

namespace GooseHtml.Tests;

public class ExampleLayoutTests
{
	const string output = "<html><head></head><body><nav class=\"nav navbar\"></nav><div class=\"sidebar\"></div><footer class=\"footer\"></footer></body></html>";
	const string partialOutput = "<html><head></head><body><footer class=\"footer\"></footer></body></html>";

	[Fact]
	public void Layout_ShouldReturnHtml()
	{
		var html = new ExampleLayout();
		html.ToString().ShouldBe(output);
	}

	[Fact]
	public void PartialLayout_ShouldReturnCorrectHtml()
	{
		var html = new ExamplePartialLayout();
		html.ToString().ShouldBe(partialOutput);
	}
}

