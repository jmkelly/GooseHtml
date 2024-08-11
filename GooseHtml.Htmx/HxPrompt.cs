using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxPrompt(string Value) : Attribute("hx-prompt", Value)
{
}
