using Xunit;

public class TransactionTests
{
    [Fact]
    public void Create_ValidExpenseCategory_ReturnTransaction()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        var amount = 50000m;
        var category = Category.FoodAndDrink;

        // Act
        var transaction = Transaction.Create(date, amount, category, null);

        // Assert
        Assert.Equal(date, transaction.Date);
        Assert.Equal(amount, transaction.Amount);
        Assert.Equal(category, transaction.Category);
    }

    [Fact]
    public void Create_NegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 1);
        var amount = -40000m;
        var category = Category.FoodAndDrink;

        // Assert
        Assert.Throws<ArgumentException>(() => Transaction.Create(date, amount, category, null));
    }
}
