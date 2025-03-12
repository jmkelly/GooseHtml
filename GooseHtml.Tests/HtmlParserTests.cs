
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

		element.ToString().ShouldContain("Hello");

	}

    [Fact]
    public void Should_Handle_Multiple_Nested_Elements()
    {
        // Arrange
        string html = "<div><p><b>Bold</b> Text</p></div>";
        var parser = new HtmlParser(html);

        // Act
        var element = parser.Parse();

        // Assert
        element.ShouldNotBeNull();
        element.ShouldBeOfType<Div>();

        var divChildren = element.Elements;
        divChildren.Count.ShouldBe(1);
        divChildren[0].ShouldBeOfType<P>();

        var pChildren = divChildren[0].Elements;
        pChildren.Count.ShouldBe(1);
        pChildren[0].ShouldBeOfType<B>();

        pChildren[0].ToString().ShouldContain("Text");
    }
}

