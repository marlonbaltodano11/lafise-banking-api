using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingApi.Application.Interfaces;
using BankingApi.Domain.Entities;

namespace BankingApi.Application.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountNumberGenerator _accountNumberGenerator;

        public AccountService(IAccountRepository accountRepository, ICustomerRepository customerRepository, IAccountNumberGenerator accountNumberGenerator)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _accountNumberGenerator = accountNumberGenerator;
        }

        public async Task<Account> CreateAccountAsync(Guid customerId, decimal initialBalance = 0)
        {
            if (initialBalance < 0) throw new ArgumentException("Initial balance cannot be negative");

            var customer = await _customerRepository.GetByIdAsync(customerId)
                           ?? throw new ArgumentException("Customer not found");

            var accountNumber = await _accountNumberGenerator.GenerateAsync();
            var account = new Account(customer, accountNumber, 0);

            if (initialBalance > 0)
            {
                account.Deposit(initialBalance);
            }

            await _accountRepository.AddAsync(account);
            return account;
        }

        public async Task DepositAsync(string accountNumber, decimal amount)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber)
                          ?? throw new ArgumentException("Account not found");

            account.Deposit(amount);
            await _accountRepository.UpdateAsync(account);
        }

        public async Task WithdrawAsync(string accountNumber, decimal amount)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber)
                          ?? throw new ArgumentException("Account not found");

            account.Withdraw(amount);
            await _accountRepository.UpdateAsync(account);
        }

        public async Task<decimal> GetBalanceAsync(string accountNumber)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber)
                          ?? throw new ArgumentException("Account not found");

            return account.Balance;
        }

        public async Task<Transaction[]> GetTransactionHistoryAsync(string accountNumber)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber)
                          ?? throw new ArgumentException("Account not found");

            return account.Transactions.ToArray();
        }
    }
}
