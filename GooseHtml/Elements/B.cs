namespace GooseHtml;
public class B : Element
{
    public B(): base(ElementNames.B) {}
    public B(string text) : base(ElementNames.B)
    {
        Add(new Text(text));
    }
}
