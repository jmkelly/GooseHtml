namespace GooseHtml.Attributes;

public record Method(HttpRequestType RequestType):Attribute(Name:"method", Value: RequestType.ToString().ToLowerInvariant())
{

}
