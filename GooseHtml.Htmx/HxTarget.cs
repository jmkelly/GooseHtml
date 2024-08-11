using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxTarget(string Value) : Attribute("hx-target", Value)
{
}
