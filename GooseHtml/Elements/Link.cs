namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : Element
{
    public Link()
    {
    }

    public Link(string href) : base("link", [new Href(href)])
	{
	}

    public Link(string href, string rel) : base("link", [new Href(href), new Attribute("rel", rel)])
    {
    }
}
