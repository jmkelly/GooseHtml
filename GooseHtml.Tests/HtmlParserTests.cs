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
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
        element.AsT0.ShouldBeOfType<Div>();
    }

    [Fact]
    public void Should_Parse_Element_With_Attributes()
    {
        // Arrange
        string html = "<div class='container' id='main'></div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
        element.AsT0.ShouldBeOfType<Div>();

		element.MatchT0(e => e.Attributes.Count.ShouldBe(2));
		//extend test to test the attributes
		element.MatchT0(e => e.Attributes[0].Name.ShouldBe("class"));
		element.MatchT0(e => e.Attributes[0].Value.ShouldBe("container"));
		element.MatchT0(e => e.Attributes[1].Name.ShouldBe("id"));
		element.MatchT0(e => e.Attributes[1].Value.ShouldBe("main"));
    }

    [Fact]
    public void Should_Parse_Nested_Elements()
    {
        // Arrange
        string html = "<div><p>Hello</p></div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
		var el = element.AsT0;
		el.ShouldBeOfType<Div>();
        /**/

		var children = el.Elements;
		children.ShouldNotBeEmpty();
        children.Count.ShouldBe(1);
		children[0].AsT0.ShouldBeOfType<P>();
    }

    [Fact]
    public void Should_Parse_Self_Closing_Element()
    {
        // Arrange
        string html = "<img src='image.png' />";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
        element.AsT1.ShouldBeOfType<Img>();

        element.AsT1.Attributes[0].Value.ShouldBe("image.png");
        element.AsT1.Attributes[0].Name.ShouldBe("src");
        element.AsT1.Attributes[0].ShouldBeOfType<Src>();
    }

	[Fact]
	public void Should_Parse_Text_Inside_Element()
	{
		string html = "<div>Hello</div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

		element.ToString().ShouldContain("Hello");

	}

    [Fact]
    public void Should_Handle_Multiple_Nested_Elements()
    {
        // Arrange
        string html = "<div><p><b>Bold</b> Text</p></div>";
        var parser = new HtmlParser(html);

        // Act
        var div = parser.Parse();

        // Assert
        div.AsT0.ShouldBeOfType<Div>();

        var divElements = div.AsT0.Elements;
        divElements.Count.ShouldBe(1);
        divElements[0].ShouldBeOfType<P>();
		divElements[0].ToString().ShouldContain("Text");

        var pChildren = divElements[0].AsT0.Elements;
		//text counts as an element
        pChildren.Count.ShouldBe(2);
        pChildren[0].AsT0.ShouldBeOfType<B>();

        pChildren[0].ToString().ShouldContain("Bold");
    }


	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_Doctype()
	{
		var html = "<!DOCTYPE html><html></html>";
		var parser = new HtmlParser(html);
		var element = parser.Parse();
		element.AsT0.ShouldBeOfType<Html>();
	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_DoctypeWhenSpaces()
	{
		var html = @"<!DOCTYPE html>  
			
			<html></html>";
		var parser = new HtmlParser(html);
		var element = parser.Parse();
		element.AsT0.ShouldBeOfType<Html>();
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
				</head>
				<body>
				</body>
			</html>";
		var parser = new HtmlParser(html);
		var element = parser.Parse();
		element.AsT0.ShouldBeOfType<Html>();

	}
}

