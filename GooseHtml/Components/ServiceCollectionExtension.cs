using Microsoft.Extensions.DependencyInjection;

namespace GooseHtml.Components;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddComponents(this IServiceCollection services)
	{
		services.AddSingleton<IComponentRegistry, ComponentRegistry>();
		return services;
	}
}

