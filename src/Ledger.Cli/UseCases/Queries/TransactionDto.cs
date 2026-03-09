using Ledger.Domain.Enums;

namespace Ledger.Cli.UseCases.Queries;

public record TransactionDto(DateOnly Date, decimal Amount, Category Category, string? Note);
