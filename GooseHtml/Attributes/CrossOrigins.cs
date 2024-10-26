namespace GooseHtml.Attributes;

public record CrossOrigin(string Value) : Attribute("crossorigin", Value);
public record CrossOriginAnonymous(): CrossOrigin("anonymous");
public record CrossOriginUseCredentials(): CrossOrigin("use-credentials");

public static class CrossOrigins
{
	public static CrossOriginAnonymous Anonymous() => new();
	public static CrossOriginUseCredentials UseCredentials() => new();
}

