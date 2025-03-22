using GooseHtml.Attributes;
using Shouldly;

namespace GooseHtml.Tests;

public class HtmlParserTests
{
    [Fact]
    public void Should_Parse_Single_Element()
    {
        // Arrange
        string html = "<div></div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
        element.ShouldNotBeNull();
        element.ShouldBeOfType<Div>();
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
        element.ShouldNotBeNull();
        element.ShouldBeOfType<Div>();
		element.Attributes.Count.ShouldBe(2);
		//extend test to test the attributes
		element.Attributes[0].Name.ShouldBe("class");
		element.Attributes[0].Value.ShouldBe("container");
		element.Attributes[1].Name.ShouldBe("id");
		element.Attributes[1].Value.ShouldBe("main");
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
        element.ShouldNotBeNull();
        element.ShouldBeOfType<Div>();
        
        var children = element.Elements;
        children.ShouldNotBeEmpty();
        children.Count.ShouldBe(1);
        children[0].ShouldBeOfType<P>();
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
        element.ShouldNotBeNull();
        element.ShouldBeOfType<Img>();

        element.Attributes[0].Value.ShouldBe("image.png");
        element.Attributes[0].Name.ShouldBe("src");
        element.Attributes[0].ShouldBeOfType<Src>();
    }

	[Fact]
	public void Should_Parse_Text_Inside_Element()
	{
		string html = "<div>Hello</div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

		element?.ToString().ShouldContain("Hello");

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
        div.ShouldNotBeNull();
        div.ShouldBeOfType<Div>();

        var divElements = div.Elements;
        divElements.Count.ShouldBe(1);
        divElements[0].ShouldBeOfType<P>();
		divElements[0].ToString().ShouldContain("Text");

        var pChildren = divElements[0].Elements;
		//text counts as an element
        pChildren.Count.ShouldBe(2);
        pChildren[0].ShouldBeOfType<B>();

        pChildren[0].ToString().ShouldContain("Bold");
    }


	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_Doctype()
	{
		var html = "<!DOCTYPE html><html></html>";
		var parser = new HtmlParser(html);
		var element = parser.Parse();
		element.ShouldBeOfType<Html>();
	}

	[Fact]
	[Trait("Category", "parser")]
	public void Should_Handle_Elements_Under_DoctypeWhenSpaces()
	{
		var html = @"<!DOCTYPE html>  
			
			<html></html>";
		var parser = new HtmlParser(html);
		var element = parser.Parse();
		element.ShouldBeOfType<Html>();
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
		element.ShouldBeOfType<Html>();

		//notes on implementation
		//1.  create a void element with no end tag
		//2.  create all the void elements that extend void
		//3. within the parser, check for void element, if it is found, ignore trying to parse the end tag
		//https://html.spec.whatwg.org/multipage/syntax.html#void-elements
		//void elements are:
		//area, base, br, col, embed, hr, img, input, link, meta, source, track, wbr
	}
}

