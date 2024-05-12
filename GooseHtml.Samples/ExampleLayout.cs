using Microsoft.AspNetCore.Mvc;


namespace GooseHtml.Samples;

//wire up some layout template code to see how it feels
//
public class ExampleLayout : Html
{
    public ExampleLayout(): base("html")
    {
		this.Add(Head());
		this.Add(Body());
    }


	public ExampleLayout(Element element) : base("html")
	{
		this.Add(Head());
		this.Add(Body(element));
	}


	public Body Body(Element element)
	{

		var body = new Body();
		body.Add(new Nav(new Class("nav navbar")));
		body.Add(new Div(new Class("sidebar")));
		var content = new Div(new Class("content"));
		content.Add(element);
		body.Add(content);
		body.Add(new Footer(new Class("footer")));
		return body;
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

    public IActionResult ToActionResult()
    {
        throw new NotImplementedException();
    }
}
