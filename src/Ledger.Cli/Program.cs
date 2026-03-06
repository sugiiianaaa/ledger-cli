using Ledger.Infrastructure;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<LedgerDbContext>()
    .UseSqlite("Data Source=ledger.db")
    .Options;

await using var db = new LedgerDbContext(options);

Console.WriteLine("Application started...");
