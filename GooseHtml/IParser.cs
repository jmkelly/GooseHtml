namespace GooseHtml;

public interface IParser
{
	Either<Element, Element> Parse();
}


