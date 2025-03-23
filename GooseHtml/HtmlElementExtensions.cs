namespace GooseHtml;

public static class HtmlElementExtensions
{
    public static bool IsElement(this OneOf<Element, VoidElement> result) => result.IsT0;
    public static bool IsVoidElement(this OneOf<Element, VoidElement> result) => result.IsT1;

    public static Element AsElement(this OneOf<Element, VoidElement> result) => result.AsT0;
    public static VoidElement AsVoidElement(this OneOf<Element, VoidElement> result) => result.AsT1;

    public static void Match(
        this OneOf<Element, VoidElement> result,
        Action<Element> elementAction,
        Action<VoidElement> voidElementAction
    ) 
	{
		result.Match(elementAction, voidElementAction);
	}

	public static void MatchElement(this OneOf<Element, VoidElement> result, Action<Element> elementAction)
	{
		result.MatchT0(elementAction);
	}

	public static void MatchVoidElement(this OneOf<Element, VoidElement> result, Action<VoidElement> voidElementAction)
	{
		result.MatchT1(voidElementAction);
	}


}

