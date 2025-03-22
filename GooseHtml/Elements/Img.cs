namespace GooseHtml;
using GooseHtml.Attributes;

public class Img : VoidElement
{
	public Img(string src, string alt) : base("img", selfClosing: false)
	{
		Add(new Src(src));
		Add(new Alt(alt));
	}

    public Img(bool selfClosing = false) : base("img", selfClosing)
    {
    }
}

