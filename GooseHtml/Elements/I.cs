namespace GooseHtml;
public class I : Element
{
    public I() : base(ElementNames.I)
    {
    }
    public I(string text) : base(ElementNames.I)
    {
        Add(new Text(text));
    }
}
