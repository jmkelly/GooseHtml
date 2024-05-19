namespace GooseHtml;

public class Script : Element
{
	public Script() : base("script")
	{
	}

	public Script(string src) : base("script", new [] {new Attribute("src", src)})
	{
	}
}

public class Link : Element
{
	public Link(string src) : base("link", new [] {new Attribute("href", src)})
	{
	}
}
