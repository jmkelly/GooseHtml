namespace GooseHtml.Htmx;

public record HxDisable(string value) : Attributes.Attribute("hx-disable", value)
{
}
