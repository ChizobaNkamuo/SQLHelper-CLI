using Spectre.Console;

namespace SqlHelper;

public static class CliHelpers
{
    public static void PrintTable<T>(IEnumerable<T> rows)
    {
        var props = typeof(T).GetProperties(); // Get the rows of the passed object
        var table = new Table();

        foreach (var p in props)
            table.AddColumn(p.Name);

        foreach (var row in rows) // Convert each item in each row into a string
            table.AddRow(
                props.Select(p => p.GetValue(row)?.ToString() ?? "").ToArray()
            );

        AnsiConsole.Write(table);
    }


    public static async Task ParseParameters(Dictionary<string, Func<ExportOptions, Task>> VALID_QUERIES, Dictionary<string, List<string>> QUERY_PARAMETERS,  
                                HashSet<string> VALID_SYMBOLS, Dictionary<string, string> given_parameters)
    {
        Func<ExportOptions, Task> query_function;
        given_parameters.TryGetValue("--query", out var query);
        if (query == null)
        {
            Console.WriteLine("No query provided.");
            return;
        }
        else if (!VALID_QUERIES.TryGetValue(query, out query_function))
        {
            Console.WriteLine("Invalid query");
            ListQueries(VALID_QUERIES, QUERY_PARAMETERS);
            return;
        }
        var query_info = VALID_QUERIES[query];
        var query_parameters = QUERY_PARAMETERS[query];

        foreach (var parameter in query_parameters)
        {
            var parameter_string = "--" + parameter.ToLower();
            if (!given_parameters.ContainsKey(parameter_string))
            {
                Console.WriteLine($"Missing parameter '{parameter_string}'.");
                return;
            }
        }

        DateTime from = default, to = default;

        if (query_parameters.Contains("From"))
        {
            if (!DateTime.TryParse(given_parameters["--from"], out from) ||
                !DateTime.TryParse(given_parameters["--to"], out to))
            {
                Console.WriteLine("Invalid dates.");
                return;
            }else if (from > to){
                Console.WriteLine("Start date should be earlier than end date.");
                return;
            }
        }

        if (!VALID_SYMBOLS.TryGetValue(given_parameters["--symbol"], out var symbol))
        {
                Console.WriteLine("Invalid symbol.");
                ListSymbols(VALID_SYMBOLS);
                return;
        }


        var options = new ExportOptions();
        options.From = from;
        options.To = to;
        options.Symbol = symbol;
        await query_function(options);
    }

    public static Dictionary<string, string>? FormatParameters(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments were provided.");
            return null;
        }
        else if (args.Length % 2 == 0) // One parameter is missing a value
        {
            Console.WriteLine("Invalid parameter assignment.");
            return null;
        }

        var given_parameters = new Dictionary<string, string>{};
        for (int i = 1; i < args.Length; i += 2)
        {
            given_parameters.Add(args[i], args[i + 1]);
        }
        return given_parameters;
    }

    public static void ListSymbols(HashSet<string> VALID_SYMBOLS)
    {
        Console.WriteLine("Valid Symbols:");
        foreach (var symbol in VALID_SYMBOLS)
        {
            Console.WriteLine(symbol);
        }
    }

    public static void ListQueries(Dictionary<string, Func<ExportOptions, Task>> VALID_QUERIES, Dictionary<string, List<string>> QUERY_PARAMETERS)
    {
        Console.WriteLine("Valid Queries:");
        foreach (var query in VALID_QUERIES.Keys)
        {
            var parameters = string.Join(" ", QUERY_PARAMETERS[query].Select(s => "--" + s.ToLower()).ToList()); // Format parameters
            Console.WriteLine($"{query} ({parameters})");
        }
    }
}