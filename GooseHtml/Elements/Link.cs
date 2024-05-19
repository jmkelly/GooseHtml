namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : Element
{
	public Link(string href) : base("link", new [] {new Href(href)})
	{
	}

    public Link(string href, string rel) : base("link", new [] {new Href(href), new Attribute("rel", "stylesheet")})
    {
    }
}
