using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxPatch(string Value) : Attribute("hx-patch", Value)
{
}
