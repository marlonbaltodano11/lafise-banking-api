using BankingApi.Application.Interfaces;
using BankingApi.Domain.Entities;
using BankingApi.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankingDbContext _context;

        public CustomerRepository(BankingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

    }
}
