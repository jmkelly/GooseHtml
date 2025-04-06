namespace GooseHtml;

public class Section : Element
{

	public Section(Attributes.Class @class) : base(ElementNames.Section)
	{
		Add(@class);
	}


	public Section(): base(ElementNames.Section)
	{

	}
}


