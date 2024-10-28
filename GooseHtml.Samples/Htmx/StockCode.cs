class StockCode
{
	internal string Code {get;private set;}
	internal decimal Price {get;private set;}
	internal Trend Trend {get;private set;}
	internal DateTime CurrentAt {get;private set;}

	internal static StockCode Create(string code, decimal price, Trend trend, DateTime currentAt)
	{
		return new StockCode {Code = code, Price = price, Trend = trend, CurrentAt = currentAt};
	}

	internal static StockCode CreateRandomCode()
	{
		var codeStr = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3).Select(s => s[new Random().Next(s.Length)]).ToArray());

		var code = new StockCode {Code = codeStr, Price = new Random().Next(100), Trend = new Random().Next(3) == 0 ? Trend.Up : new Random().Next(3) == 1 ? Trend.Down : Trend.Flat, CurrentAt = DateTime.UtcNow};

		ArgumentNullException.ThrowIfNull(code);

		return code;
	}
}


