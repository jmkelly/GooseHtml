namespace GooseHtml;

using System.Collections.Generic;
using System.Text;
using GooseHtml.Attributes;

public class Element(string name)
{
    public readonly string Name = ValidateName(name);
    
    private static readonly Attribute[] EmptyAttributes = [];
    private static readonly Either<Element, VoidElement>[] EmptyElements = [];

    private List<Attribute>? _attributes;
    private List<Either<Element, VoidElement>>? _elements;

    public IReadOnlyList<Attribute> Attributes => _attributes ?? [.. EmptyAttributes];
    public IReadOnlyList<Either<Element, VoidElement>> Children => _elements ?? [.. EmptyElements];

    public string TagStart()
    {
        return $"<{Name}";
    }

    public string TagEnd()
    {
        return  $"</{Name}>";
    }

    public Element(string name, params Attribute[] attributes) : this(name)
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


    public void Add(Either<Element, VoidElement> element)
    {
        _elements ??= [];
        _elements.Add(element);
    }

    public void Add(Text text)
    {
        _elements ??= [];
        _elements.Add(new TextElement(text.Value));
    }

    public void Add(Class @class)
    {
        _attributes ??= [];
        _attributes.Add(@class);
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

    public void AddRange(IEnumerable<Either<Element, VoidElement>> elements)
    {
        _elements ??= [];
        _elements.AddRange(elements);
    }

    public void Remove(Either<Element, VoidElement> element)
    {
        _elements ??= [];
        _elements.Remove(element);
    }

    public string Pretty() 
    {
        return new HtmlFormatter().Pretty(ToString());
    }

    public override string ToString()
    {
        //var sb = new StringBuilder();
        var sb = StringBuilderPool.Shared.Rent();
        
        sb.Append('<').Append(Name);
        AppendAttributes(sb);
        sb.Append('>');
        if (_elements is not null)
        {
            AppendChildren(sb);
        }
        sb.Append("</").Append(Name).Append('>');
        return StringBuilderPool.Shared.GetStringAndReturn(sb);
        //return sb.ToString();
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