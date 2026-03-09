using System.Collections.Frozen;
using Ledger.Domain.Enums;

namespace Ledger.Domain.Entities;

public class Transaction
{
    public long Id { get; private set; }
    public DateOnly Date { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public Category Category { get; private set; }
    public string? Note { get; private set; }

#pragma warning disable CS8618
    private Transaction() { } // EF Core
#pragma warning restore CS8618

    /// ORM will auto-generate ID
    private Transaction(DateOnly date, decimal amount, TransactionType type, Category category, string? note)
    {
        Date = date;
        Amount = amount;
        Type = type;
        Category = category;
        Note = note;
    }

    private static readonly FrozenDictionary<Category, TransactionType> CategoryTypes = new Dictionary<Category, TransactionType>
    {
        // Expense
        { Category.FoodAndDrink, TransactionType.Expense },
        { Category.Transport, TransactionType.Expense },
        { Category.Housing, TransactionType.Expense },
        { Category.Utilities, TransactionType.Expense },
        { Category.Health, TransactionType.Expense },
        { Category.Entertainment, TransactionType.Expense },
        { Category.Shopping, TransactionType.Expense },
        { Category.Education, TransactionType.Expense },
        { Category.Travel, TransactionType.Expense },
        { Category.SocialAndGifts, TransactionType.Expense },
        { Category.Uncategorized, TransactionType.Expense },

        // Income
        { Category.Salary, TransactionType.Income },
        { Category.Freelance, TransactionType.Income },
        { Category.Investment, TransactionType.Income },
        { Category.OtherIncome, TransactionType.Income },
    }.ToFrozenDictionary();

    public static Transaction Create(DateOnly date, decimal amount, Category category, string? note)
    {
        if (amount <= 0)
            throw new ArgumentException($"{amount} is not valid. Amount must be greater than zero.");

        if (!CategoryTypes.TryGetValue(category, out var transactionType))
            throw new ArgumentException($"{category} is not supported.");

        return new Transaction(date, amount, transactionType, category, note);
    }

}
