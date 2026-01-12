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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Income)
                   .HasPrecision(18, 2);
        }
    }
}
