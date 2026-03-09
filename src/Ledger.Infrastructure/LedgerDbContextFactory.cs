using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ledger.Infrastructure;

public class LedgerDbContextFactory : IDesignTimeDbContextFactory<LedgerDbContext>
{
    public LedgerDbContext CreateDbContext(string[] args)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var dbPath = "ledger.db";
        while (dir is not null)
        {
            if (dir.GetFiles("*.sln").Length > 0)
            {
                dbPath = Path.Combine(dir.FullName, "ledger.db");
                break;
            }
            dir = dir.Parent;
        }

        var options = new DbContextOptionsBuilder<LedgerDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

        return new LedgerDbContext(options);
    }
}
