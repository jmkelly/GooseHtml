namespace GooseHtml.Tests;
using GooseHtml.Attributes;

public class HtmlExtended : Html
{
    public HtmlExtended() : base()
    {
		var head = new Head();
		var meta = new Meta();
		meta.Add(new CharsetUtf8());

		head.Add(meta);
		head.Add(new Title());


		var body = new Body();

		Add(head);
		Add(body);
		Add(new EnUsLang());
    }
}
