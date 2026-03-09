using Ledger.Domain.Entities;

namespace Ledger.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<long> AddAsync(Transaction transaction);
    Task DeleteAsync(long transactionId);
}
