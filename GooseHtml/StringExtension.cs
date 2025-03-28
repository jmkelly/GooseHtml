namespace GooseHtml;

public static class StringExtension
{
	public static string RemoveComments(this string text)
	{
			if (text.Length < 7) return text;//no comment
			//remove comments and the text between them
			var startComment = text.IndexOf("<!--");
			var endComment = text.IndexOf("-->");
			var endString = text.Length;
			if (startComment != -1 && endComment != -1)
				return $"{text[0..startComment]}{text[(endComment + 3)..endString]}";
			return text;
	}

	public static ReadOnlySpan<char> RemoveComments(this ReadOnlySpan<char> text)
	{
			if (text.Length < 7) return text;//no comment
			//remove comments and the text between them
			var startComment = text.IndexOf("<!--");
			var endComment = text.IndexOf("-->");
			var endString = text.Length;
			if (startComment != -1 && endComment != -1)
				return $"{text[0..startComment]}{text[(endComment + 3)..endString]}";
			return text;
	}
}

