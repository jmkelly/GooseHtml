namespace GooseHtml.Attributes;
public record Nonce(string value) : Attribute("nonce", value);
