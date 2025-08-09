using System.Text;

namespace GooseHtml;

public class Html : Element
{
	public Html() : base(ElementNames.Html)
	{
	}

    public override void WriteTo(StringBuilder sb)
    {
        sb.Append("<!DOCTYPE html>");
        base.WriteTo(sb);
    }

    public override string ToString()
    {
        var sb = StringBuilderPool.Shared.Rent();
        WriteTo(sb);
        return StringBuilderPool.Shared.GetStringAndReturn(sb);
    }
}