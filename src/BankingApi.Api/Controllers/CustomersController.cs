using BankingApi.Api.Contracts.Customers;
using BankingApi.Application.Interfaces;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using BankingApi.Api.Contracts.Common;

namespace BankingApi.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerRequest request)
        {
            var gender = Enum.Parse<Gender>(request.Gender, true);

            var customer = new Customer(
                request.Name,
                request.BirthDate,
                gender,
                request.Income
            );

            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(
                nameof(Create),
                new { id = customer.Id },
                new ApiResponse<CreateCustomerResponse>
                {
                    Success = true,
                    Data = new CreateCustomerResponse(customer.Id)
                }
            );
        }

    }
}
