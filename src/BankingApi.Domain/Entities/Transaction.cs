using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Enums.TransactionType Type { get; private set; }
        public decimal Amount { get; private set; }
        public decimal BalanceAfter { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid AccountId { get; private set; }

        // Constructor for EF Core ONLY
        private Transaction() { }
        public Transaction(Enums.TransactionType type, decimal amount, decimal balanceAfter, Guid accountId) 
        { 
            Id = Guid.NewGuid();
            Type = type;
            Amount = amount;
            BalanceAfter = balanceAfter;
            CreatedAt = DateTime.UtcNow;
            AccountId = accountId;
        }
    }
}
