namespace GooseHtml;

public class Title : Element
{
	public Title(string text)
	{
		Add(new Text(text));
	}
}

