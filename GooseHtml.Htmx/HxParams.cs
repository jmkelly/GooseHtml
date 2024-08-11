using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxParams(string Value) : Attribute("hx-params", Value)
{
}
