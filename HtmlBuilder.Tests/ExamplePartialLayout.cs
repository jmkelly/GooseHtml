namespace HtmlBuilder.Tests;

public class ExamplePartialLayout : ExampleLayout
{
	public override Body Body()
	{
		var body = new Body();
		body.Add(new Footer(new Class("footer")));
		return body;
	}
}

