using BankingApi.Application.Interfaces;
using BankingApi.Infrastructure.Persistence;
using BankingApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BankingDbContext>(options => 
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
