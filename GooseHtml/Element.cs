namespace GooseHtml;

using System.Collections.Generic;
using System.Text;
using GooseHtml.Attributes;

public class Element(string name, bool isVoid = false)
{
    public readonly string Name = ValidateName(name);
    public readonly bool IsVoid = isVoid;
    
    private static readonly Attribute[] EmptyAttributes = [];
    private static readonly Element[] EmptyElements = [];

    private List<Attribute>? _attributes;
    private List<Element>? _elements;

    public IReadOnlyList<Attribute> Attributes => _attributes ?? [.. EmptyAttributes];
    public IReadOnlyList<Element> Children => _elements ?? [.. EmptyElements];

    public string TagStart()
    {
        return $"<{Name}";
    }

    public string TagEnd()
    {
        return  $"</{Name}>";
    }

    public Element(string name,  bool isVoid = false, params Attribute[] attributes) : this(name, isVoid)
    {
        _attributes ??= [];
        _attributes.AddRange(attributes);
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Element name cannot be null or whitespace", nameof(name));
            
        return name;
    }


    public void Add(Element element)
    {
        _elements ??= [];
        _elements.Add(element);
    }

    public void Add(Text text)
    {
        _elements ??= [];
        _elements.Add(new TextElement(text.Value));
    }

    public void Add(Attribute attribute)
    {
        _attributes ??= [];
        _attributes.Add(attribute);
    }

	public void AddRange(IEnumerable<Attribute> attributes)
	{
        _attributes ??= [];
		_attributes.AddRange(attributes);
	}

    public void AddRange(IEnumerable<Element> elements)
    {
        _elements ??= [];
        _elements.AddRange(elements);
    }

    public void Remove(Element element)
    {
        _elements?.Remove(element);
    }

	public void Remove(Attribute attribute)
	{
		_attributes?.Remove(attribute);
	}
    public void ClearAttributes()
    {
        _attributes?.Clear();
    }

    public void ClearChildren()
    {
        _elements?.Clear();
    }

    public string Pretty() 
    {
        return new HtmlFormatter().Pretty(ToString());
    }

    public override string ToString()
    {
        var sb = StringBuilderPool.Shared.Rent();
        
        sb.Append('<').Append(Name);
        AppendAttributes(sb);
        if (IsVoid)
        {
            sb.Append('>');
            return StringBuilderPool.Shared.GetStringAndReturn(sb);
        }
        sb.Append('>');
        if (_elements is not null)
        {
            AppendChildren(sb);
        }
        sb.Append("</").Append(Name).Append('>');
        return StringBuilderPool.Shared.GetStringAndReturn(sb);
    }

    private void AppendAttributes(StringBuilder sb)
    {
        if (_attributes is null) return;
        foreach (var attr in _attributes)
        {
            sb.Append(' ')
              .Append(attr);
        }
    }

    private void AppendChildren(StringBuilder sb)
    {
        if (_elements is null) return;
        foreach (var element in _elements)
        {
            sb.Append(element);
        }
    }
}
