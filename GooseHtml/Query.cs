namespace GooseHtml;

public static class QueryExtension
{
    public static List<Element> Query<T>(this Element element) where T : Element
    {
        var result = new List<Element>();
        var children = element.Children;
        foreach (var e in children)
        {
            if (e is T)
            {
                result.Add(e);
            }
            else
            {
                var matches = e.Query<T>();
                result.AddRange(matches);
            }
        }
        return result;
    }



    public static List<Element> Query<T>(this List<Element> elements) where T : Element
    {
        var result = new List<Element>();
        foreach (var element in elements)
        {
            var matches = element.Query<T>();
            result.AddRange(matches);
        }
        return result;
    }
}