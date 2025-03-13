namespace GooseHtml.Attributes;

public record Lang(string Value): Attribute("lang", Value)
{
}

public record EnUsLang() : Attribute("lang", "en-US")
{
}

