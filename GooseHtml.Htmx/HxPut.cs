using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxPut(string Value) : Attribute("hx-put", Value)
{
}
