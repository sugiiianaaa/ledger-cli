using Ledger.Domain.Entities;
using Ledger.Domain.Enums;
using Ledger.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Ledger.Infrastructure.Tests;

public class TransactionRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly LedgerDbContext _context;
    private readonly TransactionRepository _repository;

    public TransactionRepositoryTests()
    {
        (_connection, _context) = TestDbContextFactory.CreateInMemory();
        _repository = new TransactionRepository(_context);
    }

    [Fact]
    public async Task Add_ValidTransaction_ShouldPersistAndReturnId()
    {
        // Arrange
        var transaction = Transaction.Create(
            DateOnly.FromDateTime(DateTime.UtcNow),
            50000,
            Category.Salary,
            "monthly salary");

        // Act
        var id = await _repository.AddAsync(transaction);

        // Assert
        var saved = await _context.Transactions.FindAsync(id);
        Assert.NotNull(saved);
        Assert.Equal(50000, saved.Amount);
        Assert.Equal(Category.Salary, saved.Category);
        Assert.Equal(TransactionType.Income, saved.Type);
        Assert.Equal("monthly salary", saved.Note);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
