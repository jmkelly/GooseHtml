
namespace GooseHtml;

public class Nav : Element
{

    public Nav(Attributes.Class @class) : base(ElementNames.Nav)
	{
		Add(@class);
	}

    public Nav() : base(ElementNames.Nav)
    {
    }
}	
