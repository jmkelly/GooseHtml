namespace GooseHtml.Components;

public interface IComponentRegistry
{
	Component? GetComponentByRoute(string route);
}

