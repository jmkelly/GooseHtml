namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : VoidElement
{

    public Link(string href) : base("link")
	{
		Add(new Href(href));
	}

    public Link(string href, string rel) : base("link")
    {
		Add(new Href(href));
		Add(new Rel(rel));
    }

    public Link(bool selfClosing = false) : base("link", selfClosing)
    {
    }
}
