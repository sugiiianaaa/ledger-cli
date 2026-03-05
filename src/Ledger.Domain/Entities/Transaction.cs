public class Transaction
{
    public long Id { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public Category Category { get; private set; }
    public string? Note { get; private set; }

    private Transaction(DateTime date, decimal amount, TransactionType type, Category category, string? note)
    {
        Date = date;
        Amount = amount;
        Type = type;
        Category = category;
        Note = note;
    }

    private static readonly Dictionary<Category, TransactionType> _categoryTypes = new()
    {
        // Expense
        { Category.FoodAndDrink, TransactionType.Expense},
        { Category.Transport, TransactionType.Expense},
        { Category.Housing, TransactionType.Expense},
        { Category.Utilities, TransactionType.Expense},
        { Category.Health, TransactionType.Expense},
        { Category.Entertainment, TransactionType.Expense},
        { Category.Shopping, TransactionType.Expense},
        { Category.Education, TransactionType.Expense},
        { Category.Travel, TransactionType.Expense},
        { Category.SocialAndGifts, TransactionType.Expense},
        { Category.Uncategorized, TransactionType.Expense},

        // Income
        { Category.Salary, TransactionType.Income},
        { Category.Freelance, TransactionType.Income},
        { Category.Investment, TransactionType.Income},
        { Category.OtherIncome, TransactionType.Income},
    };

    public static Transaction Create(DateTime date, decimal amount, TransactionType type, Category category, string? note)
    {
        if (_categoryTypes[category] != type)
            throw new ArgumentException($"{category} is not valid for {type}");

        return new Transaction(date, amount, type, category, note);
    }

}
