using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApi.Domain.Exceptions;

namespace BankingApi.Domain.Entities
{
    public class Account
    {
        public Guid Id {  get; private set; }
        public string AccountNumber { get; private set; } = null!;
        public Customer Owner { get; private set; } = null!;
        public decimal Balance { get; private set; }


        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        // Constructor for EF Core ONLY
        private Account() { }
        public Account(Customer owner, string accountNumber, decimal initialBalance = 0) 
        {
            Id = Guid.NewGuid();
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
            Balance = initialBalance >= 0 ? initialBalance : throw new ArgumentException("Initial balance cannot be negative");
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive");

            Balance += amount;
            var transaction = new Transaction(Enums.TransactionType.Deposit, amount, Balance);
            _transactions.Add(transaction);
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive");
            if (Balance < amount) throw new InsufficientFundsException(amount, Balance);

            Balance -= amount;
            var transaction = new Transaction(Enums.TransactionType.Withdrawal, amount, Balance);
            _transactions.Add(transaction);
        }
    }
}
