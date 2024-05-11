namespace HtmlBuilder.Tests;

//wire up some layout template code to see how it feels
//
public class Layout : Html
{
    public Layout()
    {
    }

    public override Body Body()
    {
		var body = new Body();
		body.Add(new Nav(new Class("nav navbar")));
		body.Add(new Div(new Class("sidebar")));
		body.Add(new Footer(new Class("footer")));
		return body;
    }

    public override Head Head()
    {
		return new Head();
    }
}
