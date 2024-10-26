namespace GooseHtml.Attributes;
public record Nonce(string Value) : Attribute("nonce", Value);
