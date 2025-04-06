namespace GooseHtml;
using GooseHtml.Attributes;

public class Img : VoidElement
{
	public Img(string src, string alt) : base(ElementNames.Img)
	{
		Add(new Src(src));
		Add(new Alt(alt));
	}

    public Img() : base(ElementNames.Img)
    {
    }
}

