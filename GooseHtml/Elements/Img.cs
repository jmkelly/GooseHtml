namespace GooseHtml;
using GooseHtml.Attributes;

public class Img : Element
{

	public Img(string src, string alt) : base(selfClosing: true)
	{
		Add(new Src(src));
		Add(new Alt(alt));
	}
}

