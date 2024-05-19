namespace GooseHtml;
using System.Text;
using GooseHtml.Attributes;

public record Class(string Name);

public abstract class Element
{
	internal string TagEnd;
	internal string TagStart;

	private bool SelfClosing;
	private HtmlFormatter HtmlFormatter;
	private string Name;
	private List<Attribute> Attributes;
	private List<Element> Elements; 
	private Text Text;

	public Element (Class @class, bool selfClosing=false)
	{
		this.Name = this.GetType().Name.ToLowerInvariant();
		Init(this.Name, selfClosing, new EmptyValue());
		if (Attributes != null)
		{
			Attributes.Add(new Attribute("class", @class.Name));
		}
	}

	public Element(Text value, bool selfClosing=false)
	{
		this.Name = this.GetType().Name.ToLowerInvariant();
		this.Text = value;
		Init(this.Name, selfClosing, value);
	}

	public Element(bool selfClosing=false)
	{
		this.Name = this.GetType().Name.ToLowerInvariant();
		Init(this.Name, selfClosing, new EmptyValue());
	}

	public Element(string name, bool selfClosing=false)
	{
		Init(name, selfClosing, new EmptyValue());
	}

	public Element(string name, Attribute[] attributes)
	{
		Init(name, false, new EmptyValue());
		foreach (var attribute in attributes)
		{
			Attributes.Add(attribute);
		}
	}

	public Element(string name, Attribute[] attributes, Text text)
	{
		Init(name, false, text);
		foreach (var attribute in attributes)
		{
			Attributes.Add(attribute);
		}
	}

	private void Init(string name, bool selfClosing, Text value)
	{

		this.Text = value;
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
		this.Name = name;
		this.Attributes = new List<Attribute>();
		this.Elements = new List<Element>();
		HtmlFormatter = new HtmlFormatter();
	}

	public void Add(Text text)
	{
		Text = text;
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
		return HtmlFormatter.Pretty(this.ToString());
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
		AddText(sb);
		EndTag(sb);
		return sb.ToString();
	}

    private void AddText(StringBuilder sb)
    {
		if (Text != null && Text.Value != "")//todo replace with a type check
		{
			sb.Append(Text.Value.ToString());
		}
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

	private void CloseTag(StringBuilder sb)
	{
		sb.Append(">");	
	}

	private void StartTag(StringBuilder sb)
	{
		sb.Append(TagStart);
	}

	private void AddAttrs(StringBuilder sb)
	{
		foreach (var attr in Attributes)
		{
			sb.Append(" ");
			sb.Append(attr.ToString());
		}
	}
}
