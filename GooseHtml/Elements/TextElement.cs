using System.Text;

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

	public override void WriteTo(StringBuilder sb)
	{
		if (htmlEncode)
		{
			sb.Append(System.Net.WebUtility.HtmlEncode(text));
		}
		else
		{
			sb.Append(text);
		}
	}

	public override string ToString()
	{
		var sb = StringBuilderPool.Shared.Rent();
		WriteTo(sb);
		return StringBuilderPool.Shared.GetStringAndReturn(sb);
	}

}