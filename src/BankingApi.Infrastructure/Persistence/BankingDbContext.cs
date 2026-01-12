using BankingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Infrastructure.Persistence
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext (DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankingDbContext).Assembly);
        }

    }
}
