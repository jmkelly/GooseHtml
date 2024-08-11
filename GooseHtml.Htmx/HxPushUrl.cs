using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxPushUrl(string Value) : Attribute("hx-push-url", Value)
{
}
