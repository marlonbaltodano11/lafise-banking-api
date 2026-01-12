using BankingApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Application.Services
{
    public class AccountNumberGenerator : IAccountNumberGenerator
    {
        private readonly IAccountRepository _accountRepository;
        private static readonly Random _random = new();

        public AccountNumberGenerator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string> GenerateAsync()
        {
            string accountNumber;
            bool exists;

            do
            {
                accountNumber = _random.Next(1, 10).ToString();

                for (int i = 0; i < 8; i++)
                {
                    accountNumber += _random.Next(0, 10).ToString();
                }

                // Verify if account number already exist
                var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);
                exists = account != null;

            } while (exists);

            return accountNumber;
        }
    }
}
