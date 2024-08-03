using Microsoft.AspNetCore.Builder;

namespace GooseHtml.Components;

public static class ApplicationBuilderExtension
{
	public static IApplicationBuilder UseComponents(this IApplicationBuilder app)
	{
		app.UseMiddleware<ComponentMiddleware>();
		return app;
	}
}
