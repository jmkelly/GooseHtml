namespace GooseHtml;
public class Blockquote : Element
{
    public Blockquote() : base(ElementNames.Blockquote)
    {
    }

    public Blockquote(string text) : base(ElementNames.Blockquote) 
    {
        Add(new Text(text));
    }
}
