namespace GooseHtml;

using System.Text;
using GooseHtml.Attributes;

//by default void elements dont need a closing tag
//as this saves a character, will will keep the default
//but allow overloading for self closing as well
public abstract class VoidElement 
{
	const string selfClosingTag = "/>";
	const string CloseTagChar = ">";
	const char Space = ' ';
    private readonly string TagStart;

	public IReadOnlyList<Attribute> Attributes => _attributes.AsReadOnly();
	private readonly List<Attribute> _attributes = [];
    private readonly string TagEnd;

    public VoidElement(string name, bool selfClosing=false)
	{

		TagStart = $"<{name}";
		if (selfClosing)
		{
			TagEnd = selfClosingTag;
		}
		else {
			TagEnd = CloseTagChar;
		}
    }

	public void Add(Attribute attribute)
	{
		_attributes.Add(attribute);
	}

	private void AddAttrs(StringBuilder sb)
    {
        foreach (var attribute in _attributes)
        {
			sb.Append(Space);
            sb.Append(attribute);
        }
    }

    public override string ToString()
	{
		var sb = new StringBuilder();
		sb.Append(TagStart);
		AddAttrs(sb);
		sb.Append(TagEnd);
		//we don't have a closing tag on a void element
		return sb.ToString();
	}

    internal void AddRange(List<Attribute> attributes)
    {
		_attributes.AddRange(attributes);
    }
}

