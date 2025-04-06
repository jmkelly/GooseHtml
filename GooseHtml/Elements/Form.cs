namespace GooseHtml;

public class Form : Element
{

	public Form(Attributes.Class @class) : base(ElementNames.Form)
	{
		Add(@class);
	}

	public Form(): base(ElementNames.Form)
	{

	}
}

