namespace GooseHtml;

using System.Text;
using GooseHtml.Attributes;

public class Element
{
	internal string TagEnd = string.Empty;
	internal string TagStart = string.Empty;

	private bool SelfClosing = false;
	private readonly HtmlFormatter HtmlFormatter = new();
	private readonly string Name;
	public readonly List<Attribute> Attributes = [];
	//think about removing this....and just handle with an attribute that has an empty string assigned
	private readonly List<EmptyAttribute> EmptyAttributes = [];
	public readonly List<Element> Elements = []; 

	public Element()
	{
		Name = GetType().Name.ToLowerInvariant();
		Init(Name, false );
	}

	public Element(Class @class, bool selfClosing=false)
	{
		Name = GetType().Name.ToLowerInvariant();
		Init(Name, selfClosing);
		Attributes?.Add(new Attribute("class", @class.Name));
	}

	public Element(bool selfClosing=false)
	{
		Name = GetType().Name.ToLowerInvariant();
		Init(Name, selfClosing );
	}

	public Element(string name, bool selfClosing=false)
	{
		Name = name;
		Init(name, selfClosing);
	}

	public Element(string name, Attribute[] attributes)
	{
		Name = name;
		Init(name, false);
		foreach (var attribute in attributes)
		{
			Attributes.Add(attribute);
		}
	}


	private void Init(string name, bool selfClosing)
	{
		if (selfClosing)
		{
			TagEnd = "/>";
		}
		else
		{
			TagEnd = $"</{name}>";
		}
		TagStart = $"<{name}";
		SelfClosing = selfClosing;
	}

	public void Add(Text text)
	{
		Elements.Add(new TextElement(text.Value));
	}

	public void Remove(Element element)
	{
		Elements.Remove(element);
	}

	public void Add(Class @class)
	{
		Attributes.Add(new Attribute("class", @class.Name));
	}

	public void Add(EmptyAttribute emptyAttribute)
	{
		EmptyAttributes.Add(emptyAttribute);
	}

	public void Add(Attribute attribute) 
	{
		Attributes.Add(attribute);
	}
	public void Add(Element element)
	{
		Elements.Add(element);
	}

	public void AddRange(Element[] elements)
	{
		Elements.AddRange(elements);
	}

	public string Pretty()
	{ 
		return HtmlFormatter.Pretty(ToString());
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		StartTag(sb);
		AddAttrs(sb);
		if (!SelfClosing)
		{
			CloseTag(sb);
		}

		AddElements(sb);
		EndTag(sb);
		return sb.ToString();
	}


    private void EndTag(StringBuilder sb)
	{
		sb.Append(TagEnd);
	}

	private void AddElements(StringBuilder sb)
	{
		foreach (var element in Elements)
		{
			sb.Append(element.ToString());
		}
	}

	private static void CloseTag(StringBuilder sb)
	{
		sb.Append('>');	
	}

	private void StartTag(StringBuilder sb)
	{
		sb.Append(TagStart);
	}

	private void AddAttrs(StringBuilder sb)
	{
		//set the empty elements first....
		//todo: figure out some way to add the empty and non-empty 
		//attributes in the order added to the Element
		foreach (var attr in EmptyAttributes)
		{
			sb.Append(' ');
			sb.Append(attr.ToString());
		}
		foreach (var attr in Attributes)
		{
			sb.Append(' ');
			sb.Append(attr.ToString());
		}
	}
}
