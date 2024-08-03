
namespace GooseHtml.Components;

public class ComponentRegistry : IComponentRegistry
{
    private readonly IEnumerable<Component> components;

    //register all the components
    //public static class ComponentRegistry

    public ComponentRegistry(IEnumerable<Component> components)
	{
        this.components = components;
    }



	public Component? GetComponentByRoute(string route)
	{
		if (components != null && components.Any()) 
			return components.FirstOrDefault(x => x.Route == route);
		else return null;
	}
}
