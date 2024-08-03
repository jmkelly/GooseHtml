using GooseHtml.Attributes;

namespace GooseHtml.Components;

public class FormComponent: Component
{
	private FormComponent(Form form) : base(form)
	{
		var route = this.GetType().Name.ToLowerInvariant().Replace("component", "");
		//var route = "/" + name;
		var httpRequestType = HttpRequestType.Post;
		form.Add(new Method(httpRequestType));
		form.Add(new Attributes.Action(route));
	}

	public static FormComponent Create()
	{
		var form = new Form();
		return new FormComponent(form);

	}
}

