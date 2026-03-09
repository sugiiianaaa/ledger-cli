using Ledger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Infrastructure;

public class LedgerDbContext(DbContextOptions<LedgerDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(x =>
        {
            x.HasKey(t => t.Id);
            x.Property(t => t.Type).HasConversion<string>();
            x.Property(t => t.Category).HasConversion<string>();
        });
    }
}
