namespace GooseHtml;

public class Code : Element
{
	public Code(): base(ElementNames.Code) {}

	public Code(string text): base(ElementNames.Code)
	{
		Add(new Text(text));
	}

}

