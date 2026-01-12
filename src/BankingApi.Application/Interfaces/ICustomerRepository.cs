using BankingApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task<Customer> GetByIdAsync(Guid id);
    }
}
