namespace GooseHtml;

using System.Collections.Generic;
using System.Text;
using GooseHtml.Attributes;

public class Element
{
    private readonly HtmlFormatter _htmlFormatter = new();
    private readonly string _name;
    
    private readonly List<Attribute> _attributes = [];
    private readonly List<Either<Element, VoidElement>> _elements = [];

    public IReadOnlyList<Attribute> Attributes => _attributes.AsReadOnly();
    public IReadOnlyList<Either<Element, VoidElement>> Children => _elements.AsReadOnly();

    public string TagEnd { get; }
    public string TagStart { get; }

    public Element(string name)
    {
        _name = ValidateName(name);
        (TagStart, TagEnd) = InitializeTags(_name);
    }

    public Element(string name, params Attribute[] attributes) : this(name)
    {
        _attributes.AddRange(attributes);
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Element name cannot be null or whitespace", nameof(name));
            
        return name;
    }

    private static (string Start, string End) InitializeTags(string name)
    {
        return ($"<{name}", $"</{name}>");
    }

    public void Add(Either<Element, VoidElement> element)
    {
        _elements.Add(element);
    }

    public void Add(Text text)
    {
        _elements.Add(new Either<Element, VoidElement>(new TextElement(text.Value)));
    }

    public void Add(Class @class)
    {
        _attributes.Add(@class);
    }

    public void Add(Attribute attribute)
    {
        _attributes.Add(attribute);
    }

	public void AddRange(IEnumerable<Attribute> attributes)
	{
		_attributes.AddRange(attributes);
	}

    public void AddRange(IEnumerable<Either<Element, VoidElement>> elements)
    {
        _elements.AddRange(elements);
    }

    public void Remove(Either<Element, VoidElement> element)
    {
        _elements.Remove(element);
    }

    public string Pretty() => _htmlFormatter.Pretty(ToString());

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        sb.Append(TagStart);
        AppendAttributes(sb);
        sb.Append('>');
        AppendChildren(sb);
        sb.Append(TagEnd);

        return sb.ToString();
    }

    private void AppendAttributes(StringBuilder sb)
    {
        foreach (var attr in _attributes)
        {
            sb.Append(' ')
              .Append(attr);
        }
    }

    private void AppendChildren(StringBuilder sb)
    {
        foreach (var element in _elements)
        {
            sb.Append(element);
        }
    }
}