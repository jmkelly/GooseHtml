
namespace GooseHtml.Attributes;

public record NoModule(): BooleanAttribute("nomodule");

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

