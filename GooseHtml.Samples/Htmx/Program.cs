using GooseHtml;

class Program
{
	const int numberOfStocks = 100;
	private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        List<StockCode> codes = GenerateInitialCodes();

        app.MapGet("/", () =>
        {
            var page = new HtmxPage();

            List<StockCode> replacementCodes = RegeneratePrice(codes);
            var html = page.Build(replacementCodes);
            var result = html.ToHtmlResult();

            //replace with new codes
            codes = replacementCodes;
            return result;
        });

        app.UseStaticFiles();
        app.Run();
    }

    private static List<StockCode> GenerateInitialCodes()
    {
        List<StockCode> codes = [];

        for (int i = 0; i < numberOfStocks; i++)
        {
            codes.Add(StockCode.CreateRandomCode());
        }

        return codes;
    }
    private static List<StockCode> RegeneratePrice(List<StockCode> codes)
    {
		//rather than mutate we will create a new list to avoid errors iwth concurrent 
		//viewers
        List<StockCode> replacementCodes = [];

        foreach (var code in codes)
        {
            var existingPrice = code.Price;
            var newPrice = new Random().Next(100);
            var price = newPrice;
            var trend = newPrice > existingPrice ? Trend.Up : newPrice < existingPrice ? Trend.Down : Trend.Flat;
            var currentAt = DateTime.UtcNow;

            replacementCodes.Add(new StockCode(Code: code.Code, Price: price, Trend: trend, CurrentAt: currentAt));
        }

        return replacementCodes;
    }
    internal static Div CreateGridComponent(StockCode code)
    {
        var stockCode = new H3($"{code.Code}");
        stockCode.Add(new Class("stock-code"));

        var stockPrice = new P($"{code.Price}");
        stockPrice.Add(new Class("stock-price"));


        var stockTrend = new P($"{code.Trend}");
        stockTrend.Add(new Class("stock-trend"));

        var gridItem = new Div(new Class($"grid-item {code.Trend}"));

        gridItem.Add(stockCode);
        gridItem.Add(stockPrice);
        gridItem.Add(stockTrend);
        return gridItem;
    }
}

