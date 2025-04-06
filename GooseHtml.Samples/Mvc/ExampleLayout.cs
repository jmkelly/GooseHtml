using Microsoft.AspNetCore.Mvc;


namespace GooseHtml.Samples.Mvc;

//wire up some layout template code to see how it feels
//
public class ExampleLayout : Html
{
    public ExampleLayout(): base()
    {
		Add(Head());
		Add(Body());
    }


	public ExampleLayout(Element element) : base()
	{
		Add(Head()); 
		Add(Body(element));
	}


	public Body Body(Element element)
	{

		var body = new Body();
		body.Add(new Nav(new Attributes.Class("nav navbar")));
		body.Add(new Div(new Attributes.Class("sidebar")));
		var content = new Div(new Attributes.Class("content"));
		content.Add(element);
		body.Add(content);
		body.Add(new Footer(new Attributes.Class("footer")));
		return body;
	}
    public virtual Body Body()
    {
		var body = new Body();
		body.Add(new Nav(new Attributes.Class("nav navbar")));
		body.Add(new Div(new Attributes.Class("sidebar")));
		body.Add(new Footer(new Attributes.Class("footer")));
		return body;
    }

    public virtual Head Head()
    {
		var head = new Head();

		return head;
    }
}
