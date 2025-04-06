
namespace GooseHtml;

public class Html : Element
{
	public Html() : base(ElementNames.Html)
	{
	}

    public override string ToString()
    {
		//add the html doctype
		return  base.ToString().Insert(0, "<!DOCTYPE html>");
    }
}

