namespace GooseHtml;

public interface IParser
{
	OneOf<Element, VoidElement> Parse();
}


