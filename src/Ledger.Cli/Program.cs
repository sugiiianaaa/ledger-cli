using System.Text;
using Ledger.Cli.Commands;
using Ledger.Cli.Infrastructure;
using Ledger.Cli.UseCases.Commands;
using Ledger.Cli.UseCases.Queries;
using Ledger.Domain.Interfaces;
using Ledger.Infrastructure;
using Ledger.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

var dbPath = ResolveDbPath();

static string ResolveDbPath()
{
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    while (dir is not null)
    {
        if (dir.GetFiles("*.sln").Length > 0)
            return Path.Combine(dir.FullName, "ledger.db");
        dir = dir.Parent;
    }

    // Fallback: current working directory
    return Path.Combine(Directory.GetCurrentDirectory(), "ledger.db");
}

var services = new ServiceCollection();

services.AddDbContext<LedgerDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

services.AddScoped<ITransactionRepository, TransactionRepository>();
services.AddScoped<IAddTransaction, AddTransaction>();
services.AddScoped<GetTransactionsByMonth>();

// Ensure database is created and migrations applied
using (var sp = services.BuildServiceProvider())
{
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<LedgerDbContext>();
    await db.Database.MigrateAsync();
}

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.PropagateExceptions();
    config.AddCommand<AddCommand>("add")
        .WithDescription("Add a new transaction")
        .WithExample("add", "50000", "--note", "lunch")
        .WithExample("add", "4000000", "--note", "salary", "--category", "Salary");

    config.AddCommand<ListCommand>("list")
        .WithDescription("List transactions by month")
        .WithExample("list", "2026", "3");
});

// If arguments were passed, run as a single command (non-interactive)
if (args.Length > 0)
{
    return await app.RunAsync(args);
}

// Interactive REPL mode
AnsiConsole.MarkupLine("[bold green]Ledger CLI[/] — type [yellow]help[/] for commands, [yellow]exit[/] to quit.");

while (true)
{
    AnsiConsole.Markup("[blue]ledger>[/] ");
    var line = Console.ReadLine();

    if (line is null) // Ctrl+D
        break;

    var input = line.Trim();

    if (input is "")
        continue;

    if (input is "exit" or "quit")
        break;

    try
    {
        var inputArgs = ParseArgs(input);
        await app.RunAsync(inputArgs);
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]Error:[/] {ex.Message}");
    }
}

return 0;

static string[] ParseArgs(string input)
{
    var args = new List<string>();
    var current = new StringBuilder();
    var inQuote = false;
    var quoteChar = '"';

    foreach (var c in input)
    {
        if (inQuote)
        {
            if (c == quoteChar)
            {
                inQuote = false;
            }
            else
            {
                current.Append(c);
            }
        }
        else if (c is '"' or '\'')
        {
            inQuote = true;
            quoteChar = c;
        }
        else if (c == ' ')
        {
            if (current.Length > 0)
            {
                args.Add(current.ToString());
                current.Clear();
            }
        }
        else
        {
            current.Append(c);
        }
    }

    if (current.Length > 0)
        args.Add(current.ToString());

    return args.ToArray();
}
