using Shouldly;

namespace GooseHtml.Tests;

public class HTests
{
	[Fact]
	public void H1_ShouldReturnH1()
	{
		var h1 = new H1(new Text("GooseHtml"));
		h1.ToString().ShouldBe("<h1>GooseHtml</h1>");
	}


	[Fact]
	public void H2_ShouldReturnH2()
	{
		var h2 = new H2(new Text("GooseHtml"));
		h2.ToString().ShouldBe("<h2>GooseHtml</h2>");
	}

	[Fact]
	public void H3_ShouldReturnH3()
	{
		var h3 = new H3(new Text("GooseHtml"));
		h3.ToString().ShouldBe("<h3>GooseHtml</h3>");
	}
}
