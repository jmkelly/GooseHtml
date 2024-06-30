namespace GooseHtml.Components;

public interface IComponentRegistry
{
	void Build();
	Component? GetComponentByRoute(string route);
}

