namespace GooseHtml;

using GooseHtml.Attributes;

public class Link : Element
{
	public Link(string href) : base("link", new [] {new Href(href)})
	{
	}
}
