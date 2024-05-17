using Shouldly;

namespace GooseHtml.Tests;

public class H1Tests
{
	[Fact]
	public void H1_ShouldReturnH1()
	{
		var h1 = new H1(new Text("GooseHtml"));
		h1.ToString().ShouldBe("<h1>GooseHtml</h1>");
	}
}
