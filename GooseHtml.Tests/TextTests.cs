namespace GooseHtml.Tests;

using Shouldly;

public class TextTests
{
	[Fact]
	public void Text_ShouldReturnText()
	{
		var div = new Div();
		var text = new Text("Hello World");
		var text2 = new Text("Hello World2");
		div.Add(text);
		div.Add(text2);
		div.ToString().ShouldBe("<div>Hello WorldHello World2</div>");
	}


	[Fact]
	public void Text_CanBeMixedWithOtherElements()
	{
		var div = new Div();
		var code = new Code();
		var text = new Text("Hello World");
		var text2 = new Text("Hello World2");
		code.Add(text2);
		div.Add(text);
		div.Add(code);
		div.Add(text);
		div.ToString().ShouldBe("<div>Hello World<code>Hello World2</code>Hello World</div>");
	}


	[Fact]
	public void Text_ShouldBeEncodedByDefault()
	{
		var div = new Div();
		div.Add(new Text("alert ('Hello World');"));
		div.ToString().ShouldBe("<div>alert (&#39;Hello World&#39;);</div>");
	}
}

