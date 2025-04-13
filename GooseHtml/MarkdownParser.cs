using System.Collections;
using GooseHtml;

public static class MarkdownParser
{
    public static List<Element> Parse(string markdown)
    {
        var elements = new List<Element>();

        var lines = markdown.Split('\n');
        foreach (var line in lines)
        {
            var trimmed = line.TrimEnd('\r');

            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            var element = ParseLine(trimmed);
            if (element is not null)
                elements.Add(element);
        }


        return elements;
    }

    private static Element? ParseLine(ReadOnlySpan<char> line)
    {
        if (line.StartsWith("### "))
            return new H3(line.Slice(4).ToString());
        if (line.StartsWith("## "))
            return new H2(line.Slice(3).ToString());
        if (line.StartsWith("# "))
            return new H1(line.Slice(2).ToString());
        if (line.StartsWith("- ") || line.StartsWith("* ") || line.StartsWith("+ ")) 
            return new Li(line.Slice(2).ToString());
        if (line.StartsWith("> "))
            return new Blockquote(line.Slice(2).ToString());
        return CreateParagraph(line);
    }


    private static P CreateParagraph(ReadOnlySpan<char> line)
    {
        var para = new P();
        para.AddRange(ParseInline(line));
        return para;
    }

    private static List<Element> ParseInline(ReadOnlySpan<char> line)
    {
        var result = new List<Element>();
        int i = 0;
        while (i < line.Length)
        {
            if (line[i] == '*' && i + 1 < line.Length && line[i + 1] == '*')
            {
                int end = line.Slice(i + 2).IndexOf("**");
                if (end >= 0)
                {
                    result.Add(new B(line.Slice(i + 2, end).ToString()));
                    i += end + 4;
                    continue;
                }
            }
            else if (line[i] == '*')
            {
                int end = line.Slice(i + 1).IndexOf('*');
                if (end >= 0)
                {
                    result.Add(new I ( line.Slice(i + 1, end).ToString() ));
                    i += end + 2;
                    continue;
                }
            }
            else if (line[i] == '`')
            {
                int end = line.Slice(i + 1).IndexOf('`');
                if (end >= 0)
                {
                    result.Add(new Code (line.Slice(i + 1, end).ToString() ));
                    i += end + 2;
                    continue;
                }
            }

            // Add plain text
            int next = FindNextSpecial(line.Slice(i));
            result.Add(new TextElement (line.Slice(i, next).ToString()));
            i += next;
        }

        return result;
    }

    private static int FindNextSpecial(ReadOnlySpan<char> text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] is '*' or '`')
                return i;
        }
        return text.Length;
    }
}
