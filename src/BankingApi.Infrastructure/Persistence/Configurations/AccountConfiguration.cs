using BankingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccountNumber)
                   .IsRequired();

            builder.HasIndex(x => x.AccountNumber)
                   .IsUnique();

            builder.Property(x => x.Balance)
                   .HasPrecision(18, 2);

            builder.HasOne(x => x.Owner)
                   .WithMany()
                   .IsRequired();

            builder.HasMany(x => x.Transactions)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
