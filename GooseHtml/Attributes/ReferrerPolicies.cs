
namespace GooseHtml.Attributes;

// from https://developer.mozilla.org/en-US/docs/Web/HTML/Element/script#referrerpolicy
// no-referrer: The Referer header will not be sent.
// no-referrer-when-downgrade: The Referer header will not be sent to origins without TLS (HTTPS).
// origin: The sent referrer will be limited to the origin of the referring page: its scheme, host, and port.
// origin-when-cross-origin: The referrer sent to other origins will be limited to the scheme, the host, and the port. Navigations on the same origin will still include the path.
// same-origin: A referrer will be sent for same origin, but cross-origin requests will contain no referrer information.
// strict-origin: Only send the origin of the document as the referrer when the protocol security level stays the same (HTTPS→HTTPS), but don't send it to a less secure destination (HTTPS→HTTP).
// strict-origin-when-cross-origin (default): Send a full URL when performing a same-origin request, only send the origin when the protocol security level stays the same (HTTPS→HTTPS), and send no header to a less secure destination (HTTPS→HTTP).
// unsafe-url
//
public abstract record ReferrerPolicy(string value): Attribute("referrerpolicy", value);

public record NoReferrer(): ReferrerPolicy("no-referrer");
public record NoReferrerWhenDowngrade(): ReferrerPolicy("no-referrer-when-downgrade");
public record Origin(): ReferrerPolicy("origin");
public record OriginWhenCrossOrigin(): ReferrerPolicy("origin-when-cross-origin");
public record SameOrigin(): ReferrerPolicy("same-origin");
public record StrictOrigin(): ReferrerPolicy("strict-origin");
public record StrictOriginWhenCrossOrigin(): ReferrerPolicy("strict-origin-when-cross-origin");
public record UnsafeUrl(): ReferrerPolicy("unsafe-url");

public static class ReferrerPolicies
{
	public static NoReferrer NoReferrer() => new NoReferrer();
	public static NoReferrerWhenDowngrade NoReferrerWhenDowngrade() => new NoReferrerWhenDowngrade();
	public static Origin Origin() => new Origin();
	public static OriginWhenCrossOrigin OriginWhenCrossOrigin() => new OriginWhenCrossOrigin();
	public static SameOrigin SameOrigin() => new SameOrigin();
	public static StrictOrigin StrictOrigin() => new StrictOrigin();
	public static StrictOriginWhenCrossOrigin StrictOriginWhenCrossOrigin() => new StrictOriginWhenCrossOrigin();
	public static UnsafeUrl UnsafeUrl() => new UnsafeUrl();
}


