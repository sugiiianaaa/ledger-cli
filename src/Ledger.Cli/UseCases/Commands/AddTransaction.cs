using Ledger.Domain.Entities;
using Ledger.Domain.Interfaces;

namespace Ledger.Cli.UseCases.Commands;

public interface IAddTransaction
{
    Task<long> ExecuteAsync(AddTransactionInput input);
}

public class AddTransaction(ITransactionRepository transactionRepository) : IAddTransaction
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;

    public async Task<long> ExecuteAsync(AddTransactionInput input)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);

        var transaction = Transaction.Create(
            date,
            input.Amount,
            input.Category,
            input.Note
        );

        return await _transactionRepository.AddAsync(transaction);
    }
}
