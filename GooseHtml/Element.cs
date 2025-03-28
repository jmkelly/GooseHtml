namespace GooseHtml;

using System.Text;
using GooseHtml.Attributes;


public class Element 
{
	internal string TagEnd = string.Empty;
	internal string TagStart = string.Empty;

	private readonly HtmlFormatter HtmlFormatter = new();
	private readonly string Name;
	public readonly List<Attribute> Attributes = [];
	public readonly List<Either<Element, VoidElement>> Elements = [];

    public List<Either<Element, VoidElement>> Children => [.. Elements];
    public void Add(Either<Element, VoidElement> element)
    {
		Elements.Add(element);
    }

	public Element(string name)
	{
		Name = name;
		Init(Name );
	}

	public Element(string name, Attribute[] attributes)
	{
		Name = name;
		Init(name);
		foreach (var attribute in attributes)
		{
			Attributes.Add(attribute);
		}
	}


	private void Init(string name )
	{
		TagEnd = $"</{name}>";
		TagStart = $"<{name}";
	}

	public void Add(Text text)
	{
		Elements.Add(new Either<Element, VoidElement>(new TextElement(text.Value)));
	}

	public void Remove(Either<Element, VoidElement> element)
	{
		Elements.Remove(element);
	}

	public void Add(Class @class)
	{
		Attributes.Add(@class);
	}

	public void Add(Attribute attribute) 
	{
		Attributes.Add(attribute);
	}

	public void AddRange(List<Either<Element, VoidElement>> elements)
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
		CloseTag(sb);
		AddElements(sb);
		EndTag(sb);
		return sb.ToString();
	}


    internal void EndTag(StringBuilder sb)
	{
		sb.Append(TagEnd);
	}

	internal void AddElements(StringBuilder sb)
	{
		foreach (var element in Elements)
		{
			sb.Append(element.ToString());
		}
	}

	internal static void CloseTag(StringBuilder sb)
	{
		sb.Append('>');	
	}

	internal void StartTag(StringBuilder sb)
	{
		sb.Append(TagStart);
	}

	internal void AddAttrs(StringBuilder sb)
	{
		foreach (var attr in Attributes)
		{
			sb.Append(' ');
			sb.Append(attr.ToString());
		}
	}
}
