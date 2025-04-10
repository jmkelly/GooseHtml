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

		var children = el.Children;
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

		var divElements = result.AsElement().Children;
		divElements.Count.ShouldBe(1);
		divElements[0].AsElement().ShouldBeOfType<P>();
		divElements[0].ToString().ShouldContain("Text");

		var pChildren = divElements[0].AsElement().Children;
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
		var head = either.AsElement().Children[0].AsElement();
		head.ShouldBeOfType<Head>();
		head.Children.Count.ShouldBe(4);
		head.Children[0].AsVoidElement().ShouldBeOfType<Meta>();
		head.Children[1].AsVoidElement().ShouldBeOfType<Meta>();
		head.Children[2].AsVoidElement().ShouldBeOfType<Meta>();
		head.Children[3].AsVoidElement().ShouldBeOfType<Meta>();

	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Ignore_Insignificant_Whitespace()
	{
		string html = "<div>\n    <p>Hello</p>\n    <p>World</p>\n</div>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		var div = result.AsElement();
		div.Children.Count.ShouldBe(2); // Only <p> elements, no Text nodes
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
			span.Children.Count.ShouldBe(1);
			var text = span.Children[0].AsElement();
			text.ShouldBeOfType<TextElement>();
			text.ToString().ShouldContain("Feed refreshed Mar 22");
			text.ToString().ShouldNotContain("<!--");
			text.ToString().ShouldNotContain("-->");
		}

		[Fact]
		[Trait("Category","parser")]
		public void Should_Parse_Without_Closing_Tag()
		{
			//is is still compliant
			string html = "<html><body><h1></h1><p>end of document</p>";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			var element = result.AsElement();
			element.ShouldBeOfType<Html>();
			var body = element.Children[0].AsElement();
			body.ShouldBeOfType<Body>();
			body.Children.Count.ShouldBe(2);

		}

		[Fact]
		[Trait("Category","parser")]
		public void Script_ShouldRenderCorrectly()
		{
			var text = "alert('Hello <world>!');";
			Script script = new();
			script.Add(new TextElement(text, false));
			script.ToString().ShouldBe("<script>alert('Hello <world>!');</script>");
		}

		[Fact]
		[Trait("Category","parser")]
		public void DD_ShouldntNeedClosingTagWhenImmediatelyFollowedByAnotherDD()
		{
			var html = @"
			<dl>
				<dt>first heading
				<dd><p>first paragraph within dd</p>
				<dt>second heading
				<dd><p>second paragraph within dd</p>
			</dl>";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			var dl = result.AsElement();
			dl.Children.Count.ShouldBe(4);
			var firstDt = dl.Children[0].AsElement();
			var secondDt = dl.Children[2].AsElement();
			firstDt.ShouldBeOfType<Dt>();
			secondDt.ShouldBeOfType<Dt>();

		}

		[Fact]
		[Trait("Category","parser")]
		public void P_ShouldntNeedClosingTagWhenImmediatelyFollowedByAnotherP()
		{
			var html = @"
			<div>
				<p>first paragraph
				<p>second paragraph	
			</div>";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			var div = result.AsElement();
			div.Children.Count.ShouldBe(2);
			var firstparagrah = div.Children[0].AsElement();
			var secondparagraph = div.Children[1].AsElement();
			firstparagrah.ShouldBeOfType<P>();
			secondparagraph.ShouldBeOfType<P>();

		}
}
