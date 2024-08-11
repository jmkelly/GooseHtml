using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxVars(string Value) : Attribute("hx-vars", Value)
{
}
