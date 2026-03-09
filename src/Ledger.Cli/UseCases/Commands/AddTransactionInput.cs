using Ledger.Domain.Enums;

namespace Ledger.Cli.UseCases.Commands;

public record AddTransactionInput(decimal Amount, Category Category, string? Note);
