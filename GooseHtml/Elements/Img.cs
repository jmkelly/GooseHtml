namespace GooseHtml;
using GooseHtml.Attributes;

public class Img : Element
{
	public Img() : base(selfClosing: true) { }

	public Img(string src, string alt) : base(selfClosing: true)
	{
		Add(new Src(src));
		Add(new Alt(alt));
	}
}

