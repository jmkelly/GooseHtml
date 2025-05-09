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
			element.ShouldBeOfType<Div>();
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
		result.ShouldBeOfType<Div>();

		result.Attributes.Count.ShouldBe(2);
		//extend test to test the attributes
		result.Attributes[0].Name.ShouldBe("class");
		result.Attributes[0].Value.ShouldBe("container");
		result.Attributes[1].Name.ShouldBe("id");
		result.Attributes[1].Value.ShouldBe("main");
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
		var el = element;
		el.ShouldBeOfType<Div>();
		/**/

		var children = el.Children;
		children.ShouldNotBeEmpty();
		children.Count.ShouldBe(1);
		children[0].ShouldBeOfType<P>();
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
		result.ShouldBeOfType<Img>();

		result.Attributes[0].Value.ShouldBe("image.png");
		result.Attributes[0].Name.ShouldBe("src");
		result.Attributes[0].ShouldBeOfType<Src>();
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
		result.ShouldBeOfType<Div>();

		var divElements = result.Children;
		divElements.Count.ShouldBe(1);
		divElements[0].ShouldBeOfType<P>();
		divElements[0].ToString().ShouldContain("Text");

		var pChildren = divElements[0].Children;
		//text counts as an element
		pChildren.Count.ShouldBe(2);
		pChildren[0].ShouldBeOfType<B>();

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
		element.ShouldBeOfType<Html>();
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
		element.ShouldBeOfType<Html>();
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
		either.ShouldBeOfType<Html>();
		var head = either.Children[0];
		head.ShouldBeOfType<Head>();
		head.Children.Count.ShouldBe(4);
		head.Children[0].ShouldBeOfType<Meta>();
		head.Children[1].ShouldBeOfType<Meta>();
		head.Children[2].ShouldBeOfType<Meta>();
		head.Children[3].ShouldBeOfType<Meta>();

	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Ignore_Insignificant_Whitespace()
	{
		string html = "<div>\n    <p>Hello</p>\n    <p>World</p>\n</div>";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		var div = result;
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
		result.ShouldBeOfType<Div>();
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
		result.ShouldBeOfType<Div>();
	}

	[Fact]
	[Trait("Category","parser")]
	public void Should_Parse_Unquoted_Attribute_With_Space_Before_SelfClose()
	{
		string html = "<input value=test />";
		var parser = new HtmlParser(html);
		var result = parser.Parse();
		result.Attributes.ShouldContain(a => a.Name == "value" && a.Value == "test");
	}

	[Fact]
	[Trait("Category","parser")]
	//this may not be technically correct according to the spec.....but it _seems_ right
		public void Should_Parse_Unquoted_Href_Attribute()
		{
			string html = "<a href=https://google.com />";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			result.Attributes.ShouldContain(a => a.Name == "href" && a.Value == "https://google.com");
		}

	[Fact]
	[Trait("Category","parser")]
		public void Should_Parse_Element_With_Comments()
		{
			string html = "<span>Feed refreshed <!-- -->Mar 22</span>";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			var span = result;
			span.Children.Count.ShouldBe(1);
			var text = span.Children[0];
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
			var element = result;
			element.ShouldBeOfType<Html>();
			var body = element.Children[0];
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
			var dl = result;
			dl.Children.Count.ShouldBe(4);
			var firstDt = dl.Children[0];
			var secondDt = dl.Children[2];
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
			var div = result;
			div.Children.Count.ShouldBe(2);
			var firstparagrah = div.Children[0];
			var secondparagraph = div.Children[1];
			firstparagrah.ShouldBeOfType<P>();
			secondparagraph.ShouldBeOfType<P>();

		}

		[Fact]
		[Trait("Category","parser")]
		public void Ul_ShouldntParseProperly()
		{
			var html = """
				<ul class="search-suggest">
				  <li data-lon="143.484" data-lat="-19.986">
				    <a href="/places/qld/woolgar/">Woolgar, QLD 4822</a>
				  </li>
				  <li data-lon="148.734" data-lat="-34.890">
				    <a href="/places/nsw/woolgarlo/">Woolgarlo, NSW 2582</a>
				  </li>
				  <li data-lon="153.200" data-lat="-30.111">
				    <a href="/places/nsw/woolgoolga/">Woolgoolga, NSW 2456</a>
				  </li>
				  <li data-lon="115.810" data-lat="-27.698">
				    <a href="/places/wa/woolgorong/">Woolgorong, WA 6630</a>
				  </li>
				</ul>
				""";
			var parser = new HtmlParser(html);
			var result = parser.Parse();
			result.Pretty().ShouldBe(html);
		}

}
