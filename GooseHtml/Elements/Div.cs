namespace GooseHtml;

public class Div : Element
{
	public Div(Attributes.Class @class) : base(ElementNames.Div)
	{
		Add(@class);
	}


	public Div(): base(ElementNames.Div)
	{

	}

}	
