namespace GooseHtml;

public interface IParser
{
	Either<Element, VoidElement> Parse();
}


