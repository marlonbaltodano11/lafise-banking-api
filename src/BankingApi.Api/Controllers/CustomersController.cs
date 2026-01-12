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

        /// <summary>
        /// Registers a new customer in the system.
        /// </summary>
        /// <param name="request">The customer registration details containing personal and financial information.</param>
        /// <returns>The newly created customer's unique identifier.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CreateCustomerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCustomerRequest request)
        {
            if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
            {
                throw new ArgumentException("Invalid gender value");
            }

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
