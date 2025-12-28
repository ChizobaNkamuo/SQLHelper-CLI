# SQLHelper CLI

A C# CLI tool for querying commodity price data from an SQL Server using stored procedures.

## Features
- Dapper-based async SQL execution
- Strongly typed DTO mapping
- Reflection-based table output
- Extensible query and export architecture

## Commands
- dry-run: execute queries and print results
- queries: list supported queries
- symbols: list valid symbols
- export: (work in progress)

## Example Usage
```C#
dotnet run dry-run --query GetPrices --symbol Gold --from 2024-01-01 --to 2024-01-31
```
```C#
dotnet run symbols
```
```C#
dotnet run queries
```

## Planned
- CSV / json export
- Configurable connection strings
- Additional datasets
