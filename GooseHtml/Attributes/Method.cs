namespace GooseHtml.Attributes;

/*public record Method(HttpRequestType RequestType):Attribute(Name:"method", Value: RequestType.ToString().ToLowerInvariant())*/
/*{*/
/**/
/*}*/

//this should probably have been a class, but then it messes with the whole thing
//revisit if there are more of these cases where OO style is preferable
public record Method(string Value): Attribute("method", Value)
{

}
