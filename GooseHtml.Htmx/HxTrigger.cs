using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxTrigger(string Value) : Attribute("hx-trigger", Value)
{
}
