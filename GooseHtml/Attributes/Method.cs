namespace GooseHtml.Attributes;

/*public class Method(HttpRequestType RequestType):Attribute(Name:"method", Value: RequestType.ToString().ToLowerInvariant())*/
/*{*/
/**/
/*}*/

//this should probably have been a class, but then it messes with the whole thing
//revisit if there are more of these cases where OO style is preferable
public class Method(string Value): Attribute("method", Value)
{

}
