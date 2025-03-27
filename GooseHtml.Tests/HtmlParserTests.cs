using Dumpify;
using GooseHtml.Attributes;
using Shouldly;

namespace GooseHtml.Tests;

public class HtmlParserTests
{
	//https://html.spec.whatwg.org/multipage/syntax.html#void-elements
	[Fact]
	public void Should_Parse_Single_Element()
	{
		// Arrange
		string html = "<div></div>";
		var parsers = ParserFactory.CreateAll(html);
		//var parser = new HtmlParser(html);

		// Act
		foreach (var parser in parsers)
		{
			var element = parser.Parse();
			// Assert
			element.AsElement().ShouldBeOfType<Div>();
		}
	}

	[Fact]
	public void Should_Parse_Element_With_Attributes()
	{
		// Arrange
		string html = "<div class='container' id='main'></div>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{


		// Act
		var result = parser.Parse();

		// Assert
		result.AsElement().ShouldBeOfType<Div>();

		result.MatchElement(e => e.Attributes.Count.ShouldBe(2));
		//extend test to test the attributes
		result.MatchElement(e => e.Attributes[0].Name.ShouldBe("class"));
		result.MatchElement(e => e.Attributes[0].Value.ShouldBe("container"));
		result.MatchElement(e => e.Attributes[1].Name.ShouldBe("id"));
		result.MatchElement(e => e.Attributes[1].Value.ShouldBe("main"));
		}
	}

	[Fact]
	public void Should_Parse_Nested_Elements()
	{
		// Arrange
		string html = "<div><p>Hello</p></div>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{

		// Act
		var element = parser.Parse();

		// Assert
		var el = element.AsElement();
		el.ShouldBeOfType<Div>();
		/**/

		var children = el.Elements;
		children.ShouldNotBeEmpty();
		children.Count.ShouldBe(1);
		children[0].AsElement().ShouldBeOfType<P>();
		}
	}

	[Fact]
	public void Should_Parse_Self_Closing_Element()
	{
		// Arrange
		string html = "<img src='image.png' />";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{

		// Act
		var result = parser.Parse();

		// Assert
		result.AsVoidElement().ShouldBeOfType<Img>();

		result.AsVoidElement().Attributes[0].Value.ShouldBe("image.png");
		result.AsVoidElement().Attributes[0].Name.ShouldBe("src");
		result.AsVoidElement().Attributes[0].ShouldBeOfType<Src>();
		}
	}

	[Fact]
	public void Should_Parse_Text_Inside_Element()
	{
		string html = "<div>Hello</div>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{

			// Act
			var result = parser.Parse();

			result.ToString().ShouldContain("Hello");
		}

	}

	[Fact]
	public void Should_Handle_Multiple_Nested_Elements()
	{
		// Arrange
		string html = "<div><p><b>Bold</b> Text</p></div>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{

		// Act
		var result = parser.Parse();

		// Assert
		result.AsElement().ShouldBeOfType<Div>();

		var divElements = result.AsElement().Elements;
		divElements.Count.ShouldBe(1);
		divElements[0].AsElement().ShouldBeOfType<P>();
		divElements[0].ToString().ShouldContain("Text");

		var pChildren = divElements[0].AsElement().Elements;
		//text counts as an element
		pChildren.Count.ShouldBe(2);
		pChildren[0].AsElement().ShouldBeOfType<B>();

		pChildren[0].ToString().ShouldContain("Bold");
		}
	}


	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_Doctype()
	{
		var html = "<!DOCTYPE html><html></html>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{
		var element = parser.Parse();
		element.AsElement().ShouldBeOfType<Html>();
		}
	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_DoctypeWhenSpaces()
	{
		var html = @"<!DOCTYPE html>  

			<html></html>";
		var parsers = ParserFactory.CreateAll(html);
		foreach (var parser in parsers)
		{
		var element = parser.Parse();
		element.AsElement().ShouldBeOfType<Html>();
		}
	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_MetaElementsWithoutEndTags()
	{
		var html = @"<!DOCTYPE html>  
			<html>
			<head>
			<meta>
			<meta>
			<meta/>
			<meta />
			</head>
			<body>
			</body>
			</html>";
		var parser = new HtmlParser(html);
		var either = parser.Parse();
		either.AsElement().ShouldBeOfType<Html>();
		var head = either.AsElement().Elements[0].AsElement();
		head.ShouldBeOfType<Head>();
		head.Elements.Count.ShouldBe(4);
		head.Elements[0].AsVoidElement().ShouldBeOfType<Meta>();
		head.Elements[1].AsVoidElement().ShouldBeOfType<Meta>();
		head.Elements[2].AsVoidElement().ShouldBeOfType<Meta>();
		head.Elements[3].AsVoidElement().ShouldBeOfType<Meta>();

	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Ignore_Insignificant_Whitespace()
	{
		string html = "<div>\n    <p>Hello</p>\n    <p>World</p>\n</div>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		var div = result.AsElement();
		div.Elements.Count.ShouldBe(2); // Only <p> elements, no Text nodes
	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Skip_HTML_Comments()
	{
		string html = @"
			<!-- This is a comment -->
			<div>
			<!-- Another comment -->
			<p>Hello</p>
			</div>
			";
		var parser = new HtmlParser(html);
		var result = parser.Parse();

		// The root should be <div>, not a comment
		result.AsElement().ShouldBeOfType<Div>();
	}

	[Fact]
	[Trait("Category", "parser")]
	//this code fails in the ParseTag method with the error
	//Error Message: System.Exception : invalid tag name at line 1, column 1.
	//could you fix it?
	public void Should_Skip_HTML_CommentsWithOneChar()
	{
		string html = "<div><!--$!--><!--/$--></div>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();

		// The root should be <div>, not a comment
		result.AsElement().ShouldBeOfType<Div>();
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Unquoted_Attribute_With_Space_Before_SelfClose()
	{
		string html = "<input value=test />";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		result.AsVoidElement().Attributes.ShouldContain(a => a.Name == "value" && a.Value == "test");
	}

	[Fact]
	[Trait("Category","parser")]
	//this may not be technically correct according to the spec.....but it _seems_ right
		public void Should_Parse_Unquoted_Href_Attribute()
		{
			string html = "<a href=https://google.com />";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			result.AsElement().Attributes.ShouldContain(a => a.Name == "href" && a.Value == "https://google.com");
		}

	[Fact]
	[Trait("Category","parser")]
		public void Should_Parse_Element_With_Comments()
		{
			string html = "<span>Feed refreshed <!-- -->Mar 22</span>";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			var span = result.AsElement();
			span.Elements.Count.ShouldBe(1);
			var text = span.Elements[0].AsElement();
			text.ShouldBeOfType<TextElement>();
			text.ToString().ShouldContain("Feed refreshed Mar 22");
			text.ToString().ShouldNotContain("<!--");
			text.ToString().ShouldNotContain("-->");
		}
}
