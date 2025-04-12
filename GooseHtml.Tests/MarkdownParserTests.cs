using Shouldly;

namespace GooseHtml.Tests;

[Trait("Category", "markdown")]
public class MarkdownParserTests
{

    [Fact]
    public void ParsesHeadingCorrectly()
    {
        var html = MarkdownParser.Parse("# Hello World");
        html[0].ShouldBeOfType<H1>();
        var h1 = html[0];
        h1.ToString().ShouldContain("Hello World");
    }

    [Fact]
    public void ParsesItalicText()
    {
        var html = MarkdownParser.Parse("*italic*");
        var para = html[0];
        para.ShouldBeOfType<P>();
        var italic = para.Children[0];
        italic.ShouldBeOfType<I>();
        italic.ToString().ShouldContain("italic");
    }

    [Fact]
    public void ParsesBoldText()
    {
        var html = MarkdownParser.Parse("**bold**");
        var para = html[0];
        para.ShouldBeOfType<P>();
        var bold = para.Children[0];
        bold.ShouldBeOfType<B>();
        bold.ToString().ShouldContain("bold");
    }

    [Fact]
    public void ParsesInlineCode()
    {
        var html = MarkdownParser.Parse("`code`");
        var para = html[0];
        var code = para.Children[0];
        code.ShouldBeOfType<Code>();
        code.ToString().ShouldContain("code");
    }

    [Fact]
    public void ParsesListItems()
    {
        var html = MarkdownParser.Parse("- Item one\n- Item two");
        html.Count.ShouldBe(2);
        var li = html[0];
        li.ShouldBeOfType<Li>();
        li.ToString().ShouldContain("Item one");
        var li2 = html[1];
        li2.ShouldBeOfType<Li>();
        li2.ToString().ShouldContain("Item two");
    }

    [Fact]
    public void ParsesListItemsWithLineBreak()
    {
		var markdown = 
			"""
			- Item one
			- Item two 
			""";
        var html = MarkdownParser.Parse(markdown);
        html.Count.ShouldBe(2);
        var li = html[0];
        li.ShouldBeOfType<Li>();
        li.ToString().ShouldContain("Item one");
        var li2 = html[1];
        li2.ShouldBeOfType<Li>();
        li2.ToString().ShouldContain("Item two");
    }
}
