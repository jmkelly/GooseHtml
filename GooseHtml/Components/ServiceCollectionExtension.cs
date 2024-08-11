using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace GooseHtml.Components;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddComponents(this IServiceCollection services)
	{
		var componentAbstractType = typeof(Component);
        var typesFromAssemblies = Assembly.GetExecutingAssembly()
			.GetTypes()
            .Where(t => componentAbstractType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        foreach (var implementationType in typesFromAssemblies)
        {
            services.AddScoped(componentAbstractType, implementationType);
        }

		services.AddSingleton<IComponentRegistry, ComponentRegistry>();
		return services;
	}
}

