using Shouldly;

namespace GooseHtml.Tests;

[Trait("Category", "query")]
public class ElementQueryTests
{
    const string html = 
        """
        <html>
        <meta charset="utf-8">
            <title></title>
            <head></head>
            <body>
                <div name='one'>
                    <a href='places/search?p='>Example</a>
                </div>
                <div name='two'>
                    <a href='places/search?p='>Example</a>
                </div>
                <div name='one'>
                    <a href='places/search?p='>Example</a>
                </div>
            </body>
        </html>
        """;
    private Element document;

    public ElementQueryTests()
    {
        var parser = new HtmlParser(html);
        document = parser.Parse();
    }

    [Fact]
    public void Query_OfT_ReturnsListOfT()
    {
        // Act
        var result = document.Query<Div>();

        // Assert
        result.Count.ShouldBe(3);

    }

    [Fact]
    public void Query_OfT_WhenSelected_ReturnsListOfT()
    {
        // Act
        var divs = document.Query<Div>();
        var result = divs.Where(d => d.Attributes.Any(a => a is Attributes.Name && a.Value == "one"))
                         .ToList();

        // Assert
        result.Count.ShouldBe(2);

    }

    [Fact]
    public void Query_Should_AlterAttributes()
    {
        // Act
        var divs = document.Query<A>();

        var links = divs.Where(a => a.Attributes.Any(a => a is Attributes.Href)).ToList();
        foreach (var link in links)
        {
            if (link.Attributes.Any(l =>  l is Attributes.Href href))
            {
                var href = link.Attributes.First(l => l is Attributes.Href) as Attributes.Href;
                var newHref = new Attributes.Href($"https://example.com/{href.Value}");
                link.Remove(href);
                link.Add(newHref);
            }
        }

        //select all the hrefs to check
        var result = document.Query<A>().Where(a => a.Attributes.Any(a => a is Attributes.Href)).ToList();

        // Assert
        result.Count.ShouldBe(3);
        foreach (var link in result)
        {
            if (link.Attributes.Any(l => l is Attributes.Href href))
            {
                var href = link.Attributes.First(l => l is Attributes.Href) as Attributes.Href;
                href.Value.ShouldStartWith("https://example.com/");
            }
        }

    }
}
