namespace GooseHtml;

public class Title  : Element
{
    public Title(): base(ElementNames.Title)
    {
    }

    public Title(string text): base(ElementNames.Title)
	{
		Add(new Text(text));
	}

}

