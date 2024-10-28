class StockCode(string Code, decimal Price, Trend Trend, DateTime CurrentAt)
{
    public string Code { get; } = Code;
    public decimal Price { get; } = Price;
    public Trend Trend { get; } = Trend;
    public DateTime CurrentAt { get; } = CurrentAt;

    internal static StockCode CreateRandomCode()
	{
		var codeStr = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 3).Select(s => s[new Random().Next(s.Length)]).ToArray());

		var code = new StockCode (Code : codeStr, 
				Price : new Random().Next(100), 
				Trend : new Random().Next(3) == 0 ? Trend.Up : new Random().Next(3) == 1 ? Trend.Down : Trend.Flat, 
				CurrentAt : DateTime.UtcNow
			);

		ArgumentNullException.ThrowIfNull(code);

		return code;
	}
}


