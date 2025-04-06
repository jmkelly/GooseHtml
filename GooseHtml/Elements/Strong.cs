namespace GooseHtml;

public class Strong: Element
{
    public Strong(Text text) : base(ElementNames.Strong)
	{
		Add(text);
	}

    public Strong() : base(ElementNames.Strong)
    {
    }
}




