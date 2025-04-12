namespace GooseHtml;
using GooseHtml.Attributes;

public class Img : Element
{
	public Img(string src, string alt) : base(ElementNames.Img, isVoid: true)
	{
		Add(new Src(src));
		Add(new Alt(alt));
	}

    public Img() : base(ElementNames.Img, isVoid: true)
    {
    }
}

