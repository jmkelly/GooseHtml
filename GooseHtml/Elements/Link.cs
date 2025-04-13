namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : Element
{

    public Link(string href) : base(ElementNames.Link, isVoid:true)
	{
		Add(new Href(href));
	}

    public Link(string href, string rel) : base(ElementNames.Link, isVoid: true)
    {
		Add(new Href(href));
		Add(new Rel(rel));
    }

    public Link() : base("link", isVoid: true)
    {
    }
}
