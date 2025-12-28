namespace SqlHelper;

public static class QueryInfo
{
    public static readonly Dictionary<string, Func<ExportOptions, Task>> VALID_QUERIES = new()
        {
            ["GetPrices"] = Queries.GetPricesAsync,
            ["GetLatestPrice"] = Queries.GetLatestPriceAsync,
            ["GetHighestPrice"] = Queries.GetHighestPriceAsync,
            ["GetLowestPrice"] = Queries.GetLowestPriceAsync,
            ["GetDailyReturns"] = Queries.GetDailyReturnsAsync
        };

    public static readonly Dictionary<string, List<string>> QUERY_PARAMETERS = new()
        {
            ["GetPrices"] = new() { "Symbol", "From", "To" },
            ["GetLatestPrice"] = new() { "Symbol" },
            ["GetHighestPrice"] = new() { "Symbol", "From", "To" },
            ["GetLowestPrice"] = new() { "Symbol", "From", "To" },
            ["GetDailyReturns"] = new() { "Symbol", "From", "To" }
        };

    public static readonly HashSet<string> VALID_SYMBOLS = new()
        {
            "Gold",
            "Palladium",
            "Nickel",
            "Brent Oil",
            "Natural Gas",
            "US Wheat"
        };
}
