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
		var script = result.ShouldBeOfType<Script>();
		script.Attributes.ShouldContain(a => a.Name == "src" && a.Value == "app.js");
		script.Attributes.ShouldContain(a => a.Name == "async");

		// Verify script content is parsed as raw text
		script.Children.ShouldHaveSingleItem();
		var text = script.Children[0].ShouldBeOfType<TextElement>();
		text.ToString().ShouldContain("alert('Hello <world>!');");
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Empty_Script()
	{
		string html = "<script></script>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();

		result.ShouldBeOfType<Script>();
		var script = result;
		var text = script.Children[0];
		text.ToString().ShouldBeNullOrEmpty();
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Script_With_HTML_Like_Content()
	{
		string html = "<script>if (a < b) { /*...*/ }</script>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		var script = result.ShouldBeOfType<Script>();
		var text = script.Children[0].ShouldBeOfType<TextElement>();
		text.ToString().ShouldContain("if (a < b) { /*...*/ }");
		script.Children[0].ToString().ShouldContain("if (a < b) { /*...*/ }");
	}


	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Script_From_Amazon()
	{

		var html = @"<html><body><script>
		if (true === true) {
			var head = document.getElementsByTagName('head')[0],
				prefix = ""https://images-na.ssl-images-amazon.com/images/G/01/csminstrumentation/"",
				elem = document.createElement(""script"");
			elem.src = prefix + ""csm-captcha-instrumentation.min.js"";
			head.appendChild(elem);

			elem = document.createElement(""script"");
			elem.src = prefix + ""rd-script-6d68177fa6061598e9509dc4b5bdd08d.js"";
			head.appendChild(elem);
		}
		</script>
	</body></html>";
		var parser = new HtmlParser(html);
		parser.Parse();
	}

	[Theory]
	[Trait("Category","parser")]
	[InlineData('\n')]
	[InlineData('\uFEFF')]
	[InlineData(' ')]
	public void Should_Parse_WhenLastCharIsEmpty(char empty)
	{

		var html = $"<html><body></body></html>{empty}";
		var parser = new HtmlParser(html);
		parser.Parse();
	}

	[Theory]
	[Trait("Category","parser")]
	[InlineData('\n')]
	//[InlineData('\uFEFF')]
	//[InlineData(' ')]
	public void Should_Parse_WhenFirstCharIsEmpty(char empty)
	{

		var html = $"{empty}<html><body></body></html>";
		var parser = new HtmlParser(html);
		parser.Parse();
	}

	[Theory]
	[Trait("Category","parser")]
	[InlineData("<!DOCTYPE html><html class=\"a-no-js\" lang=\"en-us\"><!--<![endif]--><head></head><body></body></html>")]
	public void Should_Ignore_DocType(string html)
	{
		var parser = new HtmlParser(html);
		parser.Parse();
	}

	[Theory]
	[Trait("Category","parser")]
	[InlineData("<!DOCTYPE html><html class=\"a-no-js\" lang=\"en-us\"><!--<![endif]--><head></head><body><!--F#1-->this is text<!--another comment--></body></html>")]
	[InlineData("<input value=\"test\"></input")]
	[InlineData("<template><input slot=\"input\" name=\"password-existing\" type=\"password\" placeholder=\"Enter existing password\" autocomplete=\"off\"></input></template>")]
	[InlineData("<html><div></br></div>")]
    [InlineData("<button class=\"toggle-control\"><svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 448 512\" class=\"icon filter-white\" role=\"menu\"><title>open navigation menu</title><path d=\"M16 132h416c8.837 0 16-7.163 16-16V76c0-8.837-7.163-16-16-16H16C7.163 60 0 67.163 0 76v40c0 8.837 7.163 16 16 16zm0 160h416c8.837 0 16-7.163 16-16v-40c0-8.837-7.163-16-16-16H16c-8.837 0-16 7.163-16 16v40c0 8.837 7.163 16 16 16zm0 160h416c8.837 0 16-7.163 16-16v-40c0-8.837-7.163-16-16-16H16c-8.837 0-16 7.163-16 16v40c0 8.837 7.163 16 16 16z\"/ ></svg></button>")]
	public void Should_ParseFunky(string html)
	{
		var parser = new HtmlParser(html);
		parser.Parse();
	}

}

