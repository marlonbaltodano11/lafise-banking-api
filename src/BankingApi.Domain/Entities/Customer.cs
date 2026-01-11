using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Enums.Gender Gender { get; private set; }
        public decimal Income { get; private set; }

        public Customer(string name, DateTime birthDate, Enums.Gender gender, decimal income)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            BirthDate = birthDate;
            Gender = gender;
            Income = income;
        }
    }
}
