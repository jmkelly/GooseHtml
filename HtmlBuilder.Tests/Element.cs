using System.Text;

namespace HtmlBuilder.Tests;

public abstract class Element
{
	internal string TagEnd;
	internal string TagStart;

	private bool AutoClose;
	private HtmlFormatter HtmlFormatter;
	private string Name;
	private List<Attr> Attrs;
	private List<Element> Elements; 
	private Text Text;
	public Element(Text value, bool autoClose=false)
	{
		this.Name = this.GetType().Name.ToLowerInvariant();
		Init(this.Name, autoClose, value);
	}

	public Element(bool autoClose=false)
	{
		this.Name = this.GetType().Name.ToLowerInvariant();
		Init(this.Name, autoClose, new EmptyValue());
	}

	public Element(string name, bool autoClose=false)
	{
		Init(name, autoClose, new EmptyValue());
	}

	private void Init(string name, bool autoClose, Text value)
	{

		this.Text = value;
		if (autoClose)
		{
			TagEnd = $"</{name}>";
		}
		else
		{
			TagEnd = "/>";
		}
		TagStart = $"<{name}";
		AutoClose = autoClose;
		this.Name = name;
		this.Attrs = new List<Attr>();
		this.Elements = new List<Element>();
		HtmlFormatter = new HtmlFormatter();
	}

	public void Add(Attr attr) 
	{
		Attrs.Add(attr);
	}
	public void Add(Element element)
	{
		Elements.Add(element);
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
		if (AutoClose)
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
		foreach (var attr in Attrs)
		{
			sb.Append(" ");
			sb.Append(attr.ToString());
		}
	}
}
