using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxSwap(string Value) : Attribute("hx-swap", Value)
{
}
