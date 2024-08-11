using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxSse(string Value) : Attribute("hx-sse", Value)
{
}
