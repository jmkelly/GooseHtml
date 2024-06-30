using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace GooseHtml.Components;

public static class ApplicationBuilderExtension
{
	public static IApplicationBuilder UseComponents(this IApplicationBuilder app)
	{
		var componentRegistry = app.ApplicationServices.GetRequiredService<IComponentRegistry>();
		componentRegistry.Build(); 
		app.UseMiddleware<ComponentMiddleware>();
		return app;
	}
}
