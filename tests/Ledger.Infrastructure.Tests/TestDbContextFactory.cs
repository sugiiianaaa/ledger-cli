using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Infrastructure.Tests;

public static class TestDbContextFactory
{
    public static (SqliteConnection Connection, LedgerDbContext Context) CreateInMemory()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<LedgerDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new LedgerDbContext(options);
        context.Database.EnsureCreated();

        return (connection, context);
    }
}
