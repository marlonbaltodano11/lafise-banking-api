using BankingApi.Domain.Entities;
using BankingApi.Domain.Enums;
using BankingApi.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace BankingApi.Domain.Tests.Entities
{
    public class AccountTests
    {

        [Fact]
        public void Constructor_WithValidInitialBalance_CreatesAccount()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var accountNumber = "123456789";
            var initialBalance = 1000m;

            // Act
            var account = new Account(customer, accountNumber, initialBalance);

            // Assert
            Assert.NotNull(account);
            Assert.Equal(customer, account.Owner);
            Assert.Equal(accountNumber, account.AccountNumber);
            Assert.Equal(initialBalance, account.Balance);
            Assert.Empty(account.Transactions);
        }

        [Fact]
        public void Constructor_WithNegativeInitialBalance_ThrowsException()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var accountNumber = "123456789";
            var initialBalance = -100m;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Account(customer, accountNumber, initialBalance)
            );
        }

        [Fact]
        public void Deposit_WithValidAmount_IncreasesBalance()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var initialBalance = 100m;
            var depositAmount = 50m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.Deposit(depositAmount);

            // Assert
            Assert.Equal((initialBalance + depositAmount), account.Balance);
        }

        [Fact]
        public void Deposit_WithValidAmount_RegistersTransaction()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var initialBalance = 100m;
            var depositAmount = 50m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.Deposit(depositAmount);

            // Assert
            Assert.Single(account.Transactions);

            var transaction = account.Transactions.First();
            Assert.Equal(TransactionType.Deposit, transaction.Type);
            Assert.Equal(depositAmount, transaction.Amount);
            Assert.Equal((initialBalance + depositAmount), transaction.BalanceAfter);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Deposit_WithInvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 100m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => account.Deposit(amount));
        }

        [Fact]
        public void Withdraw_WithValidAmount_DecreasesBalance()
        {
            // Arrage
            var customer = CreateDummyCustomer();
            var initialBalance = 100m;
            var withdrawAmount = 50m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.Withdraw(withdrawAmount);

            // Assert
            Assert.Equal((initialBalance - withdrawAmount), account.Balance);
        }

        [Fact]
        public void Withdraw_WithValidAmount_RegistersTransaction()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var initialBalance = 1000m;
            var withdrawAmount = 250m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.Withdraw(withdrawAmount);

            // Assert
            Assert.Single(account.Transactions);

            var transaction = account.Transactions.First();
            Assert.Equal(TransactionType.Withdrawal, transaction.Type);
            Assert.Equal(withdrawAmount, transaction.Amount);
            Assert.Equal((initialBalance - withdrawAmount), transaction.BalanceAfter);
        }

        [Fact]
        public void Withdraw_WithInsufficientFunds_ThrowsException()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var initialBalance = 100m;
            var withdrawAmount = 150m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act & Assert
            Assert.Throws<InsufficientFundsException>(() =>
                account.Withdraw(withdrawAmount)
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Withdraw_WithInvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 1000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                account.Withdraw(amount)
            );
        }

        [Fact]
        public void ApplyInterest_WithValidRate_IncreasesBalance()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var initialBalance = 1000m;
            var interestRate = 0.05m; // 5%
            var expectedBalance = 1050m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.ApplyInterest(interestRate);

            // Assert
            Assert.Equal(expectedBalance, account.Balance);
        }

        [Fact]
        public void ApplyInterest_WithValidRate_RegistersTransaction()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var initialBalance = 1000m;
            var interestRate = 0.10m; // 10%
            var expectedInterestAmount = 100m; // 1000 * 10%
            var expectedBalance = 1100m;

            var account = new Account(customer, "123456789", initialBalance);

            // Act
            account.ApplyInterest(interestRate);

            // Assert
            Assert.Single(account.Transactions);

            var transaction = account.Transactions.First();
            Assert.Equal(TransactionType.InterestApplied, transaction.Type);
            Assert.Equal(expectedInterestAmount, transaction.Amount); 
            Assert.Equal(expectedBalance, transaction.BalanceAfter);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-0.05)]
        public void ApplyInterest_WithInvalidRate_ThrowsArgumentException(decimal rate)
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 1000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                account.ApplyInterest(rate)
            );
        }

        [Fact]
        public void Balance_AfterMultipleOperations_ReturnsCorrectValue()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 1000m);

            // Act
            account.Deposit(500m);    // 1500
            account.Withdraw(200m);   // 1300
            account.ApplyInterest(0.10m); // 1430

            // Assert
            Assert.Equal(1430m, account.Balance);
        }

        [Fact]
        public void Transactions_AfterOperations_ContainsAllTransactions()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 1000m);

            // Act
            account.Deposit(200m);
            account.Withdraw(100m);
            account.ApplyInterest(0.05m);

            // Assert
            Assert.Equal(3, account.Transactions.Count);
        }

        [Fact]
        public void Transactions_AreRecordedInCorrectOrder()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            var account = new Account(customer, "123456789", 1000m);

            // Act
            account.Deposit(300m);        // 1300
            account.Withdraw(200m);       // 1100
            account.ApplyInterest(0.10m); // 1210

            // Assert
            var transactions = account.Transactions.ToList();

            Assert.Equal(TransactionType.Deposit, transactions[0].Type);
            Assert.Equal(1300m, transactions[0].BalanceAfter);

            Assert.Equal(TransactionType.Withdrawal, transactions[1].Type);
            Assert.Equal(1100m, transactions[1].BalanceAfter);

            Assert.Equal(TransactionType.InterestApplied, transactions[2].Type);
            Assert.Equal(1210m, transactions[2].BalanceAfter);
        }

        private Customer CreateDummyCustomer()
        {
            return new Customer(
                name: "John Doe",
                birthDate: new DateTime(1990, 1, 1),
                gender: Gender.Male,
                income: 2000m
            );
        }
    }
}
