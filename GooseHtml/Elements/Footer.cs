
namespace GooseHtml;

public class Footer : Element
{
    public Footer(): base(ElementNames.Footer)
    {
    }

    public Footer(Attributes.Class @class) : base(ElementNames.Footer)
	{
		Add(@class);
	}
}	
