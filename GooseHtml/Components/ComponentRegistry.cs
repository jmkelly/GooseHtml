using System.Reflection;

namespace GooseHtml.Components;

public class ComponentRegistry : IComponentRegistry
{
	//register all the components
	//public static class ComponentRegistry
	private List<Component> _components = new List<Component>();

	public ComponentRegistry()
	{
	}

	public void Build()
	{
		var componentAbstractType = typeof(Component);

		var componentTypes = Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(x => componentAbstractType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

		foreach (var componentType in componentTypes)
		{
			Register((Activator.CreateInstance(componentType) as Component)!);
		}

	}

	public void Register(Component component)
	{
		_components.Add(component);
	}

	public Component? GetComponentByRoute(string route)
	{
		if (_components != null &&_components.Any()) 
			return _components.FirstOrDefault(x => x.Route == route);
		else return null;
	}
}
