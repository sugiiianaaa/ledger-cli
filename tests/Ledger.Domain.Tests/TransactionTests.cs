using Ledger.Domain.Entities;
using Ledger.Domain.Enums;
using Xunit;

namespace Ledger.Domain.Tests;

public class TransactionTests
{
    [Fact]
    public void Create_ValidExpenseCategory_ShouldReturnTransaction()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        const decimal amount = 50000m;
        const Category category = Category.FoodAndDrink;

        // Act
        var transaction = Transaction.Create(date, amount, category, null);

        // Assert
        Assert.Equal(date, transaction.Date);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(category, transaction.Category);
        Assert.Equal(TransactionType.Expense, transaction.Type);
    }

    [Fact]
    public void Create_ValidIncomeCategory_ShouldReturnTransaction()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        const decimal amount = 50000m;
        const Category category = Category.Salary;

        // Act
        var transaction = Transaction.Create(date, amount, category, null);

        // Assert
        Assert.Equal(date, transaction.Date);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(category, transaction.Category);
        Assert.Equal(TransactionType.Income, transaction.Type);
    }

    [Fact]
    public void Create_ZeroAmount_ShouldThrow()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        const decimal amount = 0;
        const Category category = Category.FoodAndDrink;

        // Assert
        Assert.Throws<ArgumentException>(() => Transaction.Create(date, amount, category, null));
    }

    [Fact]
    public void Create_NegativeAmount_ShouldThrow()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        const decimal amount = -40000m;
        const Category category = Category.FoodAndDrink;

        // Assert
        Assert.Throws<ArgumentException>(() => Transaction.Create(date, amount, category, null));
    }
}
