using DotNetCoreCurrencyApi.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreCurrencyApi.Data
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeTransaction>(entity =>
            {
                entity.ToTable("ExchangeTransactions");
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TransactionUtcDate).IsRequired();
                entity.Property(e => e.OriginAmount).IsRequired().HasColumnType("decimal(18, 2)");
                entity.Property(e => e.OriginCurrencyCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.DestinationCurrencyCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.PurchasedAmount).IsRequired().HasColumnType("decimal(18, 5)");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currencies");
                entity.Property(e => e.Code).IsRequired().HasMaxLength(3);
                entity.Property(e => e.TransactionLimitPerMonth).IsRequired().HasColumnType("decimal(18, 2)");
                entity.Property(e => e.RateApiEndpoint).IsRequired().HasMaxLength(300);
                entity.Property(e => e.RestEnabled).IsRequired();
                entity.Property(e => e.USDRateBase).IsRequired().HasColumnType("decimal(18, 2)");
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<ExchangeTransaction> ExchangeTransactions { get; set; }
    }
}
