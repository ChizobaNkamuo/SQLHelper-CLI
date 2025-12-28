using SqlHelper;

var command = args[0];
var VALID_QUERIES = QueryInfo.VALID_QUERIES;
var QUERY_PARAMETERS = QueryInfo.QUERY_PARAMETERS;
var VALID_SYMBOLS = QueryInfo.VALID_SYMBOLS;

switch (command)
{
    case "export":
        Console.WriteLine("Export functionality is a work in progress.");
        Console.WriteLine("Currently supported: dry-run, queries, symbols.");
        break;
    case "dry-run":
        var given_parameters = CliHelpers.FormatParameters(args);
        if (given_parameters == null)
        {
            return;
        }
        
        await CliHelpers.ParseParameters(VALID_QUERIES, QUERY_PARAMETERS, VALID_SYMBOLS, given_parameters);
        break;
    case "symbols":
        CliHelpers.ListSymbols(VALID_SYMBOLS);
        break;
    case "queries":
        CliHelpers.ListQueries(VALID_QUERIES, QUERY_PARAMETERS);
        break;
    default:
        Console.WriteLine("Unknown command.");
        break;
}
