namespace GooseHtml.Attributes;

//note that according to the documentation at https://developer.mozilla.org/en-US/docs/Web/HTML/Element/script/type
//if the type is for javascript, then it is encouraged to leave out, so we are not going to provide an
//option for this default
//
public abstract record Type(string value): Attribute("type", value);

public record ModuleType(): Type("module");
public record ImportMapType(): Type("importmap");

public static class Types
{
	public static Type Module() => new ModuleType();
	public static Type ImportMap() => new ImportMapType();
}

    // src — Address of the resource 
    // type — Type of script 
    // nomodule — Prevents execution in user agents that support module scripts 
    // async — Execute script when available, without blocking while fetching 
    // defer — Defer script execution 
    // crossorigin — How the element handles crossorigin requests 
    // integrity — Integrity metadata used in Subresource Integrity checks [SRI] 
    // referrerpolicy — Referrer policy for fetches initiated by the element 
    // blocking — Whether the element is potentially render-blocking 
    // fetchpriority — Sets the priority for fetches initiated by the element
	//

