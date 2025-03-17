
namespace GooseHtml.Attributes;

public class NoReferrer(): ReferrerPolicy("no-referrer");
public class NoReferrerWhenDowngrade(): ReferrerPolicy("no-referrer-when-downgrade");
public class Origin(): ReferrerPolicy("origin");
public class OriginWhenCrossOrigin(): ReferrerPolicy("origin-when-cross-origin");
public class SameOrigin(): ReferrerPolicy("same-origin");
public class StrictOrigin(): ReferrerPolicy("strict-origin");
public class StrictOriginWhenCrossOrigin(): ReferrerPolicy("strict-origin-when-cross-origin");
public class UnsafeUrl(): ReferrerPolicy("unsafe-url");

public static class ReferrerPolicies
{
	public static NoReferrer NoReferrer() => new();
	public static NoReferrerWhenDowngrade NoReferrerWhenDowngrade() => new();
	public static Origin Origin() => new();
	public static OriginWhenCrossOrigin OriginWhenCrossOrigin() => new();
	public static SameOrigin SameOrigin() => new();
	public static StrictOrigin StrictOrigin() => new();
	public static StrictOriginWhenCrossOrigin StrictOriginWhenCrossOrigin() => new();
	public static UnsafeUrl UnsafeUrl() => new();
}


