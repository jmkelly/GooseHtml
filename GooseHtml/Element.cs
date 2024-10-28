namespace GooseHtml;
using System.Text;
using GooseHtml.Attributes;

public abstract class Element
{
	internal string TagEnd = string.Empty;
	internal string TagStart = string.Empty;

	private bool SelfClosing = false;
	private readonly HtmlFormatter HtmlFormatter = new();
	private readonly string Name;
	private readonly List<Attribute> Attributes = [];
	private readonly List<Element> Elements = []; 


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
		foreach (var attr in Attributes)
		{
			sb.Append(' ');
			sb.Append(attr.ToString());
		}
	}
}
