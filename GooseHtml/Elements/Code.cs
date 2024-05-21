namespace GooseHtml;

public class Code : Element
{
	public Code(): base("code") {}

	public Code(string text): base("code")
	{
		Add(new Text(text));
	}

}

