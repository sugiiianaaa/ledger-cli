using Ledger.Cli.UseCases.Queries;
using Ledger.Domain.Entities;
using Ledger.Domain.Enums;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Ledger.Infrastructure.Tests;

public class GetTransactionsByMonthQueryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly LedgerDbContext _context;
    private readonly GetTransactionsByMonth _query;

    public GetTransactionsByMonthQueryTests()
    {
        (_connection, _context) = TestDbContextFactory.CreateInMemory();
        _query = new GetTransactionsByMonth(_context);
    }

    [Fact]
    public async Task Get_InvalidMonth_ShouldThrow()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _query.ExecuteAsync(0, 2026));
    }

    [Fact]
    public async Task Get_InvalidYear_ShouldThrow()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _query.ExecuteAsync(4, -2026));
    }

    [Fact]
    public async Task Get_ReturnsOnlyTransactionsInRequestedMonth()
    {
        // Arrange
        var march = Transaction.Create(new DateOnly(2026, 3, 15), 50000, Category.Salary, "march salary");
        var april = Transaction.Create(new DateOnly(2026, 4, 1), 10000, Category.FoodAndDrink, "groceries");

        _context.Transactions.AddRange(march, april);
        await _context.SaveChangesAsync();

        // Act
        var result = await _query.ExecuteAsync(3, 2026);

        // Assert
        Assert.Single(result);
        Assert.Equal(50000, result[0].Amount);
        Assert.Equal(Category.Salary, result[0].Category);
        Assert.Equal("march salary", result[0].Note);
    }

    [Fact]
    public async Task Get_NoTransactionsInMonth_ShouldReturnEmptyList()
    {
        // Arrange
        var march = Transaction.Create(new DateOnly(2026, 3, 15), 50000, Category.Salary, "salary");
        _context.Transactions.Add(march);
        await _context.SaveChangesAsync();

        // Act
        var result = await _query.ExecuteAsync(6, 2026);

        // Assert
        Assert.Empty(result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}
