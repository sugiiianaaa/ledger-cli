using Ledger.Domain.Entities;
using Ledger.Domain.Interfaces;

namespace Ledger.Infrastructure.Repositories;

public class TransactionRepository(LedgerDbContext ledgerDbContext) : ITransactionRepository
{
    private readonly LedgerDbContext _ledgerDbContext = ledgerDbContext;

    public async Task<long> AddAsync(Transaction transaction)
    {
        _ledgerDbContext.Transactions.Add(transaction);
        await _ledgerDbContext.SaveChangesAsync();

        return transaction.Id;
    }

    public async Task DeleteAsync(long transactionId)
    {
        var entity = await _ledgerDbContext.Transactions.FindAsync(transactionId)
            ?? throw new InvalidOperationException("Transaction not found.");

        _ledgerDbContext.Transactions.Remove(entity);
        await _ledgerDbContext.SaveChangesAsync();
    }
}