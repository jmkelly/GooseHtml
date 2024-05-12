namespace GooseHtml;

public class H1 : Element
{
	public H1(Text value) : base(value, selfClosing:true )
	{
	}
}


public class Input : Element
{

    public Input(): base(selfClosing:false)
	{

	}

    public Input(Type type, string name, string value) : base(selfClosing:false)
    {
		if (type == Type.Text)
		{
			this.Add(new Attribute("type", "text"));
		}
		else 
		{
			throw new NotImplementedException("type not implemented");
		}

		this.Add(new Attribute("name", name));
		this.Add(new Attribute("value", value));
    }
}

public enum Type
{
	Text
}


