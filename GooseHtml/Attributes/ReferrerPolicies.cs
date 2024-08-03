
namespace GooseHtml.Attributes;

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


