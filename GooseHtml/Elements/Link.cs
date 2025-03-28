namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : VoidElement
{

    public Link(string href) : base(ElementNames.Link)
	{
		Add(new Href(href));
	}

    public Link(string href, string rel) : base(ElementNames.Link)
    {
		Add(new Href(href));
		Add(new Rel(rel));
    }

    public Link() : base("link" )
    {
    }
}
