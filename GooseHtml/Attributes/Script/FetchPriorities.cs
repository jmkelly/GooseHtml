namespace GooseHtml.Attributes;

public record FetchPriority(string value) : Attribute("fetchpriority", value);

public record FetchPriorityHigh() : FetchPriority("high");
public record FetchPriorityLow() : FetchPriority("low");
public record FetchPriorityMedium() : FetchPriority("medium");

public static class FetchPriorities
{
	public static FetchPriorityHigh High = new FetchPriorityHigh();
	public static FetchPriorityLow Low = new FetchPriorityLow();
	public static FetchPriorityMedium Medium = new FetchPriorityMedium();
}
