using Microsoft.EntityFrameworkCore;
using Ledger.Domain.Entities;

namespace Ledger.Infrastructure;

public class LedgerDbContext : DbContext
{
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public LedgerDbContext(DbContextOptions<LedgerDbContext> options)
        : base(options) { }

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
