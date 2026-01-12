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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {

        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                   .HasPrecision(18, 2);

            builder.Property(x => x.BalanceAfter)
                   .HasPrecision(18, 2);
        }
    }
}
