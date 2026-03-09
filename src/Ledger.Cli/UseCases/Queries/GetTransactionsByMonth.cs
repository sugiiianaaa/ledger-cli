using Ledger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Cli.UseCases.Queries;

public class GetTransactionsByMonth(LedgerDbContext dbContext)
{
    private readonly LedgerDbContext _dbContext = dbContext;

    public async Task<List<TransactionDto>> ExecuteAsync(int month, int year)
    {
        if (month < 1 || month > 12)
            throw new ArgumentException($"Month : {month} is invalid");

        if (year < 1 || year > 9999)
            throw new ArgumentException($"Year : {year} is invalid");

        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1);

        return await _dbContext.Transactions
            .Where(t => t.Date >= startDate && t.Date < endDate)
            .Select(t => new TransactionDto(t.Date, t.Amount, t.Category, t.Note))
            .ToListAsync();
    }
}
