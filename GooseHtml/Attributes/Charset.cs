namespace GooseHtml;

public record Charset():Attribute("charset", "utf-8")
	{
		public override string ToString()
		{
			return base.ToString();
		}
	}

