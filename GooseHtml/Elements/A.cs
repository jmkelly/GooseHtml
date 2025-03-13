namespace GooseHtml;
using GooseHtml.Attributes;

public class A : Element
{
    public A()
    {
    }

    public A(Href href, Text text): base("a", new [] {href})
    {
		Add(text);
    }
}



