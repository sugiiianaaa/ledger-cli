using System.ComponentModel;
using Ledger.Cli.UseCases.Commands;
using Ledger.Domain.Enums;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Ledger.Cli.Commands;

public sealed class AddCommandSettings : CommandSettings
{
    [CommandArgument(0, "<amount>")]
    [Description("Transaction amount")]
    public decimal Amount { get; set; }

    [CommandOption("--note <NOTE>")]
    [Description("Optional note for the transaction")]
    public string? Note { get; set; }

    [CommandOption("--category <CATEGORY>")]
    [Description("Transaction category (interactive picker if omitted)")]
    public Category? Category { get; set; }
}

public sealed class AddCommand(IAddTransaction addTransaction) : AsyncCommand<AddCommandSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, AddCommandSettings settings, CancellationToken cancellation)
    {
        var category = settings.Category ?? PromptCategory();

        var input = new AddTransactionInput(settings.Amount, category, settings.Note);
        var id = await addTransaction.ExecuteAsync(input);

        AnsiConsole.MarkupLine($"[green]Transaction added![/] (ID: {id})");
        AnsiConsole.MarkupLine($"  Amount:   [bold]{settings.Amount:N0}[/]");
        AnsiConsole.MarkupLine($"  Category: [bold]{category}[/]");

        if (settings.Note is not null)
            AnsiConsole.MarkupLine($"  Note:     [bold]{settings.Note}[/]");

        return 0;
    }

    private static Category PromptCategory()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<Category>()
                .Title("Select a [green]category[/]:")
                .PageSize(10)
                .AddChoices(Enum.GetValues<Category>()));
    }
}
