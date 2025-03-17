namespace GooseHtml.Attributes;

public class Attribute 
{
    public Attribute(string name, string value)
    {
        Name = name;
        Value = value;
    }
	public Attribute(string name)
	{
		Name = name;
		Value = null;
	}

    public string Name { get; }
    public string? Value { get; }

    public sealed override string ToString()
	{
		if (Value is null) return Name;
		return $"{Name}=\"{Value}\"";
	}
}

