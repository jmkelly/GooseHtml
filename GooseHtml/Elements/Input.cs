namespace GooseHtml;
using GooseHtml.Attributes;

public class Input : Element
{

    public Input(): base(selfClosing:false)
	{

	}

    public Input(Type type, string name, string value) : base(selfClosing:false)
    {
		switch (type)
		{
			case Type.Text:
				Add(new Attribute("type", "text"));
				break;
			case Type.Submit:
				Add(new Attribute("type", "submit"));
				break;
			default:
				throw new NotImplementedException($"type {type} not implemented");
		}

		Add(new Attribute("name", name));
		Add(new Attribute("value", value));
    }
}
