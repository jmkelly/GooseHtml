using Microsoft.AspNetCore.Http;

namespace GooseHtml.Components;

public class ComponentMiddleware
{
	private readonly RequestDelegate next;

	public ComponentMiddleware(RequestDelegate next)
	{
		this.next = next;
	}

	public async Task Invoke(HttpContext httpContext, IComponentRegistry componentRegistry)
	{
		//see if we match a component route
		var path = httpContext.Request.Path.Value;

		var component = componentRegistry.GetComponentByRoute(path);
		if (component is not null)
		{
			var func = component.HandleAsync();
			await func(httpContext);
		}
		else
		{
			await next(httpContext);
		}
	}
}

