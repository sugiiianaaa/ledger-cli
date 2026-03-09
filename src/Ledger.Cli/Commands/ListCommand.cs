using System.ComponentModel;
using Ledger.Cli.UseCases.Queries;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Ledger.Cli.Commands;

public sealed class ListCommandSettings : CommandSettings
{
    [CommandArgument(0, "<year>")]
    [Description("Year of transaction")]
    public int Year { get; set; }

    [CommandArgument(1, "<month>")]
    [Description("Month of transaction")]
    public int Month { get; set; }
}

public sealed class ListCommand(GetTransactionsByMonth query) : AsyncCommand<ListCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, ListCommandSettings settings, CancellationToken cancellationToken)
    {
        var transactions = await query.ExecuteAsync(settings.Month, settings.Year);

        if (transactions.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No transactions found.[/]");
            return 0;
        }

        var table = new Table();
        table.AddColumn("Date");
        table.AddColumn("Category");
        table.AddColumn("Amount");
        table.AddColumn("Note");

        foreach (var t in transactions)
        {
            table.AddRow(
                t.Date.ToString("yyyy-MM-dd"),
                t.Category.ToString(),
                t.Amount.ToString("N0"),
                t.Note ?? "-");
        }

        AnsiConsole.Write(table);
        return 0;
    }
}
