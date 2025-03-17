namespace GooseHtml.Attributes;

public class Lang(string Value): Attribute("lang", Value)
{
}

public class EnUsLang() : Attribute("lang", "en-US")
{
}

