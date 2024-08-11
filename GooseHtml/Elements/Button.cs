namespace GooseHtml;

public class Button : Element
{

	public Button(): base(selfClosing:false)
	{

	}

	public Button(Text text): base()
	{
		Add(text);
	}

}




