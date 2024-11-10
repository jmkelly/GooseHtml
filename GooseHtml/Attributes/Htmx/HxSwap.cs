namespace GooseHtml.Attributes.Htmx;

public record HxSwap(string Value) : Attribute("hx-swap", Value);

public static class HxSwapValues
{
	//the default, puts the content inside the target element
	public const string InnerHTML = "innerHTML";

	//replaces the entire target element with the returned content
	public const string OuterHTML = "outerHTML";

	//prepends the content before the first child inside the target
	public const string AfterBegin = "afterbegin";

	//prepends the content before the target in the target’s parent element
	public const string BeforeBegin = "beforebegin"; 

	//appends the content after the last child inside the target
	public const string BeforeEnd = "beforeend"; 

	//appends the content after the target in the target’s parent element
	public const string AfterEnd = "afterend";

	//deletes the target element regardless of the response
	public const string Delete = "delete";

	//does not append content from response (Out of Band Swaps and Response Headers will still be processed)
	public const string None = "none";

}
