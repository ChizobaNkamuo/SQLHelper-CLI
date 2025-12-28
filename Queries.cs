using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlHelper;

public static class Queries
{
    public static async Task<IEnumerable<T>> ExecuteAsync<T>(string procedure, object parameters) // Generic handler for all SQL queries
    {
        await using var conn = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=CommoditiesDB;Trusted_Connection=True;TrustServerCertificate=True;");
        return await conn.QueryAsync<T>(
            procedure,
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    // Specific handlers for each query
    public static async Task GetPricesAsync(ExportOptions options)
    {
        var result = await ExecuteAsync<PriceDto>("GetPrices", 
        new
        {
            Symbol = options.Symbol,
            From = options.From,
            To = options.To
        }); 
        
        CliHelpers.PrintTable(result);
    }

    public static async Task GetLatestPriceAsync(ExportOptions options)
    {
        var result = await ExecuteAsync<PriceDto>("GetLatestPrice", 
        new
        {
            Symbol = options.Symbol,
        }); 

        CliHelpers.PrintTable(result);
    }

    public static async Task GetHighestPriceAsync(ExportOptions options)
    {
        var result = await ExecuteAsync<HighestPriceDto>("GetHighestPrice", 
        new
        {
            Symbol = options.Symbol,
            From = options.From,
            To = options.To
        }); 
        
        CliHelpers.PrintTable(result);
    }

    public static async Task GetLowestPriceAsync(ExportOptions options)
    {
        var result = await ExecuteAsync<LowestPriceDto>("GetLowestPrice", 
        new
        {
            Symbol = options.Symbol,
            From = options.From,
            To = options.To
        }); 
        
        CliHelpers.PrintTable(result);
    }

    public static async Task GetDailyReturnsAsync(ExportOptions options)
    {
        var result = await ExecuteAsync<DailyReturnDto>("GetDailyReturns", 
        new
        {
            Symbol = options.Symbol,
            From = options.From,
            To = options.To
        }); 
        
        CliHelpers.PrintTable(result);
    }
};

