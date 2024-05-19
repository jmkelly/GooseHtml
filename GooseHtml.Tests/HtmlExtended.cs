namespace GooseHtml.Tests;
using GooseHtml.Attributes;

public class HtmlExtended : Html
{
    public HtmlExtended() : base("html")
    {
		var head = new Head();
		var meta = new Meta();
		meta.Add(new Charset());

		head.Add(meta);
		head.Add(new Title());

		var body = new Body();

		Add(head);
		Add(body);
		Add(new Lang());
    }
}
