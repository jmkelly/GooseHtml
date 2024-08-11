using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxIndicator(string Value) : Attribute("hx-indicator", Value)
{
}
