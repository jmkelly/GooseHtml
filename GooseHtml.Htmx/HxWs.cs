using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxWs(string Value) : Attribute("hx-ws", Value)
{
}
