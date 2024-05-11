namespace HtmlBuilder.Tests;

//wire up some layout template code to see how it feels
//
public class ExampleLayout : Html
{
    public ExampleLayout(): base("html")
    {
		this.Add(Head());
		this.Add(Body());
    }

    public virtual Body Body()
    {
		var body = new Body();
		body.Add(new Nav(new Class("nav navbar")));
		body.Add(new Div(new Class("sidebar")));
		body.Add(new Footer(new Class("footer")));
		return body;
    }

    public virtual Head Head()
    {
		var head = new Head();
		return head;
    }
}
