namespace GooseHtml.Attributes;

public class CrossOrigin(string Value) : Attribute("crossorigin", Value);
public class CrossOriginAnonymous(): CrossOrigin("anonymous");
public class CrossOriginUseCredentials(): CrossOrigin("use-credentials");

public static class CrossOrigins
{
	public static CrossOriginAnonymous Anonymous() => new();
	public static CrossOriginUseCredentials UseCredentials() => new();
}

