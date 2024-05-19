namespace GooseHtml.Attributes;

public record CrossOrigin(string value) : Attribute("crossorigin", value);
public record CrossOriginAnonymous(): CrossOrigin("anonymous");
public record CrossOriginUseCredentials(): CrossOrigin("use-credentials");

public static class CrossOrigins
{
	public static CrossOriginAnonymous Anonymous() => new CrossOriginAnonymous();
	public static CrossOriginUseCredentials UseCredentials() => new CrossOriginUseCredentials();
}

