using BankingApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task AddAsync(Account account);
        Task<Account?> GetByAccountNumberAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAllAsync();
        Task UpdateAsync(Account account);
    }
}
