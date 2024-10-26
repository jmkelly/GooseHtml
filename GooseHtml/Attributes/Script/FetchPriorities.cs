namespace GooseHtml.Attributes;

public record FetchPriority(string Value) : Attribute("fetchpriority", Value);

public static class FetchPriorities
{
	public static FetchPriority High => new("high");
	public static FetchPriority Low => new("low");
	public static FetchPriority Medium => new("medium");
}
