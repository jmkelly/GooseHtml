using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GooseHtml.Components;

public abstract class Component : IComponent
{
	internal Component()
	{

	}

	public Component(Element element)
	{
		Element = element;
		Route = this.GetType().Name.ToLowerInvariant().Replace("component", "");
	}

	public string Route {get;}

	public Element Element { get; }

	public Func<HttpContext, IActionResult> Handle()
	{
		return (httpContext) =>
		{
			switch (httpContext.Request.Method)
			{
				case "GET":
					return OnGet();
				case "POST":
					return OnPost(httpContext);
				default:
					throw new NotImplementedException();
			}
		};
	}

	public virtual IActionResult OnPost(HttpContext httpContext)
	{
		return Element.ToActionResult();
	}

	public virtual IActionResult OnGet()
	{
		return Element.ToActionResult();
	}

	public virtual async Task<IActionResult> OnPostAsync(HttpContext httpContext)
	{
		return await Task.Run(() => Element.ToActionResult());
	}

	public virtual async Task<IActionResult> OnGetAsync()
	{
		return await Task.Run(() => Element.ToActionResult());
	}

	public Func<HttpContext, Task<IActionResult>> HandleAsync() 
	{
		return async (httpContext) =>
		{
			switch (httpContext.Request.Method)
			{
				case "GET":
					return await OnGetAsync();
				case "POST":
					return await OnPostAsync(httpContext);
				default:
					throw new NotImplementedException();
			}
		};
	}
}
