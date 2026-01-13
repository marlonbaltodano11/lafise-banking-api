using BankingApi.Application.Interfaces;
using BankingApi.Application.Services;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Enums;
using BankingApi.Domain.Exceptions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BankingApi.Application.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountNumberGenerator _accountNumberGenerator;
        private readonly AccountService _service;

        public AccountServiceTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _accountRepository = Substitute.For<IAccountRepository>();
            _accountNumberGenerator = Substitute.For<IAccountNumberGenerator>();

            _service = new AccountService(_accountRepository, _customerRepository, _accountNumberGenerator);
        }

        [Fact]
        public async Task CreateAccountAsync_WithValidCustomer_AddsAccount()
        {
            // Arrange
            var customer = CreateDummyCustomer();
            var initialBalance = 500m;
            var generatedAccountNumber = "123456789";

            _customerRepository
                .GetByIdAsync(customer.Id)
                .Returns(customer);

            _accountNumberGenerator
                .GenerateAsync()
                .Returns(generatedAccountNumber);

            // Act
            await _service.CreateAccountAsync(customer.Id, initialBalance);

            // Assert
            await _accountRepository.Received(1)
                .AddAsync(Arg.Is<Account>(a =>
                    a.Owner.Id == customer.Id &&
                    a.AccountNumber == generatedAccountNumber &&
                    a.Balance == initialBalance
                ));
        }

        [Fact]
        public async Task CreateAccountAsync_WithInvalidCustomer_ThrowsArgumentException()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _customerRepository
                .GetByIdAsync(customerId)
                .Returns((Customer?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateAccountAsync(customerId, 500m)
            );
        }

        [Fact]
        public async Task CreateAccountAsync_WithNegativeInitialBalance_ThrowsArgumentException()
        {
            // Arrange
            var customer = CreateDummyCustomer();

            _customerRepository
                .GetByIdAsync(customer.Id)
                .Returns(customer);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.CreateAccountAsync(customer.Id, -100m)
            );
        }

        [Fact]
        public async Task DepositAsync_WithValidAccount_UpdatesAccount()
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 1000m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act
            await _service.DepositAsync(account.AccountNumber, 300m);

            // Assert
            await _accountRepository.Received(1)
                .UpdateAsync(account);

            Assert.Equal(1300m, account.Balance);
        }

        [Fact]
        public async Task DepositAsync_WithInvalidAccount_ThrowsArgumentException()
        {
            // Arrange
            var accountNumber = "invalid-account";

            _accountRepository
                .GetByAccountNumberAsync(accountNumber)
                .Returns((Account?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.DepositAsync(accountNumber, 200m)
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public async Task DepositAsync_WithInvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 1000m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.DepositAsync(account.AccountNumber, amount)
            );
        }

        [Fact]
        public async Task WithdrawAsync_WithSufficientFunds_UpdatesAccount()
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 1000m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act
            await _service.WithdrawAsync(account.AccountNumber, 400m);

            // Assert
            await _accountRepository.Received(1)
                .UpdateAsync(account);

            Assert.Equal(600m, account.Balance);
        }

        [Fact]
        public async Task WithdrawAsync_WithInvalidAccount_ThrowsArgumentException()
        {
            // Arrange
            var accountNumber = "invalid-account";

            _accountRepository
                .GetByAccountNumberAsync(accountNumber)
                .Returns((Account?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.WithdrawAsync(accountNumber, 100m)
            );
        }

        [Fact]
        public async Task WithdrawAsync_WithInsufficientFunds_ThrowsInsufficientFundsException()
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 200m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act & Assert
            await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                _service.WithdrawAsync(account.AccountNumber, 500m)
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task WithdrawAsync_WithInvalidAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 1000m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.WithdrawAsync(account.AccountNumber, amount)
            );
        }

        [Fact]
        public async Task GetBalanceAsync_WithValidAccount_ReturnsBalance()
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 750m
            );

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act
            var balance = await _service.GetBalanceAsync(account.AccountNumber);

            // Assert
            Assert.Equal(750m, balance);
        }

        [Fact]
        public async Task GetBalanceAsync_WithInvalidAccount_ThrowsArgumentException()
        {
            // Arrange
            var accountNumber = "invalid-account";

            _accountRepository
                .GetByAccountNumberAsync(accountNumber)
                .Returns((Account?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.GetBalanceAsync(accountNumber)
            );
        }

        [Fact]
        public async Task GetTransactionsAsync_WithValidAccount_ReturnsTransactions()
        {
            // Arrange
            var account = new Account(
                accountNumber: "123456789",
                owner: CreateDummyCustomer(),
                initialBalance: 1000m
            );

            account.Deposit(200m);
            account.Withdraw(100m);

            _accountRepository
                .GetByAccountNumberAsync(account.AccountNumber)
                .Returns(account);

            // Act
            var transactions = await _service.GetTransactionHistoryAsync(account.AccountNumber);

            // Assert
            Assert.Equal(2, transactions.Count());
        }

        [Fact]
        public async Task GetTransactionsAsync_WithInvalidAccount_ThrowsArgumentException()
        {
            // Arrange
            var accountNumber = "invalid-account";

            _accountRepository
                .GetByAccountNumberAsync(accountNumber)
                .Returns((Account?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.GetTransactionHistoryAsync(accountNumber)
            );
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
