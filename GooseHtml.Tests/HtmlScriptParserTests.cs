using Dumpify;
using Shouldly;

namespace GooseHtml.Tests;

public class HtmlScriptParserTests
{
	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Script_With_Attributes_And_Content()
	{
		// Arrange
		string html = @"
			<script src='app.js' async>
			alert('Hello <world>!');
		var div = '<div></div>';
		</script>
			";
		var parser = new HtmlParser(html);

		// Act
		var result = parser.Parse();

		// Assert
		var script = result.AsElement().ShouldBeOfType<Script>();
		script.Attributes.ShouldContain(a => a.Name == "src" && a.Value == "app.js");
		script.Attributes.ShouldContain(a => a.Name == "async");

		// Verify script content is parsed as raw text
		script.Elements.ShouldHaveSingleItem();
		var text = script.Elements[0].AsElement().ShouldBeOfType<TextElement>();
		text.ToString().ShouldContain("alert('Hello <world>!');");
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Empty_Script()
	{
		string html = "<script></script>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();

		var script = result.AsElement().ShouldBeOfType<Script>();
		script.Elements.ShouldBeEmpty();
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Script_With_HTML_Like_Content()
	{
		string html = "<script>if (a < b) { /*...*/ }</script>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		var script = result.AsElement().ShouldBeOfType<Script>();
		var text = script.Elements[0].AsElement().ShouldBeOfType<TextElement>();
		text.ToString().ShouldContain("if (a < b) { /*...*/ }");
		//script.Elements[0].AsElement().ToString().ShouldContain("if (a < b) { /*...*/ }");
	}
}

