﻿using DotNetCoreCurrencyApi.Core.Domain;
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
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CurrencyCode).IsRequired().HasMaxLength(3);
                entity.Property(e => e.TransactionDate).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<ExchangeTransaction> ExchangeTransactions { get; set; }
    }
}