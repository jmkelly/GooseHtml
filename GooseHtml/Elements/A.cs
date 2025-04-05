namespace GooseHtml;
using GooseHtml.Attributes;

public class A : Element
{

    public A(Href href, Text text): base(ElementNames.A, new [] {href})
    {
		Add(text);
    }

    public A() : base(ElementNames.A)
    {
    }
}
