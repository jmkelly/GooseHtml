namespace GooseHtml;
using GooseHtml.Attributes;

public class Input : Element
{

    public Input(): base(selfClosing:false)
	{

	}

    public Input(Type type, string name, string value) : base(selfClosing:false)
    {
		if (type == Type.Text)
		{
			Add(new Attribute("type", "text"));
		}
		else 
		{
			throw new NotImplementedException("type not implemented");
		}

		Add(new Attribute("name", name));
		Add(new Attribute("value", value));
    }
}

public class Button : Element
{

	public Button(): base(selfClosing:false)
	{

	}

	public Button(Text text): base()
	{
		Add(text);
	}

}



