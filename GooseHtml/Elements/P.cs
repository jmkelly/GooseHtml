namespace GooseHtml;
using GooseHtml.Attributes;

public class P: Element
{
	public P(Text value): base()
	{
		Add(value);
	}
}

public class Ul: Element
{

	public Ul(): base("ul")
	{
	}
}

public class Li : Element
{

	public Li(): base("li")
	{
	}

	public Li(Text value) : base()
	{
		Add(value);
	}

	public Li(Element[] elements) : base("li")
	{
		AddRange(elements);
	}
}

public class A : Element
{
    public A(Href href, Text text): base("a", new [] {href})
    {
		Add(text);
    }
}


