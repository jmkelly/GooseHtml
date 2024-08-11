using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxReplaceUrl(string Value) : Attribute("hx-replace-url", Value)
{
}
