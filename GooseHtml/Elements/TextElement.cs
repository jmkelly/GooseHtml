namespace GooseHtml;

public class TextElement: Element
{
    private readonly string text;
    private readonly bool htmlEncode;

    public TextElement():base(ElementNames.Text)
    {
		text = string.Empty;
		htmlEncode = true;
    }

    //special case for text
    public TextElement(string text, bool htmlEncode=true): base(ElementNames.Text)
	{
		this.text = text;
        this.htmlEncode = htmlEncode;
    }

	public override string ToString()
	{
		if (htmlEncode)
		{
			return System.Net.WebUtility.HtmlEncode(text);
		}
		return text.ToString();
	}

}

