namespace GooseHtml;

public class Button : Element
{
    public Button():base(ElementNames.Button)
    {
    }

    public Button(Text text): base(ElementNames.Button)
	{
		Add(text);
	}

}




