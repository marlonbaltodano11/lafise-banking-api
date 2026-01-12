using BankingApi.Api.Contracts.Accounts;
using BankingApi.Api.Contracts.Common;
using BankingApi.Api.Contracts.Customers;
using BankingApi.Application.Services;
using BankingApi.Domain.Exceptions;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Creates a new bank account for a specific customer.
        /// </summary>
        /// <param name="request">The account creation details (Customer ID and Initial Balance).</param>
        /// <returns>The newly created account details.</returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<CreateAccountResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(CreateAccountRequest request)
        {
            var account = await _accountService.CreateAccountAsync(
                request.CustomerId,
                request.InitialBalance
            );

            return CreatedAtAction(
                nameof(GetBalance),
                new { accountNumber = account.AccountNumber },
                new ApiResponse<CreateAccountResponse>
                {
                    Success = true,
                    Data = new CreateAccountResponse(
                        account.AccountNumber,
                        account.Balance
                    )
                }
            );
        }

        /// <summary>
        /// Retrieves the current balance for a specific account.
        /// </summary>
        /// <param name="accountNumber">The unique account number.</param>
        /// <returns>The current balance information.</returns>
        [HttpGet("{accountNumber}/balance")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<BalanceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBalance(string accountNumber)
        {
            var balance = await _accountService.GetBalanceAsync(accountNumber);

            return Ok(
                new ApiResponse<BalanceResponse>
                {
                    Success = true,
                    Data = new BalanceResponse(balance)
                }
            );
        }

        /// <summary>
        /// Deposits funds into a specific account.
        /// </summary>
        /// <param name="accountNumber">The unique account number.</param>
        /// <param name="request">The amount to be deposited.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("{accountNumber}/deposit")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deposit(string accountNumber, AmountRequest request)
        {
            await _accountService.DepositAsync(accountNumber, request.Amount);
            return NoContent();
        }

        /// <summary>
        /// Withdraws funds from a specific account.
        /// </summary>
        /// <param name="accountNumber">The unique account number.</param>
        /// <param name="request">The amount to be withdrawn.</param>
        /// <returns>No content if successful.</returns>
        [HttpPost("{accountNumber}/withdraw")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Withdraw(string accountNumber, AmountRequest request)
        {
            await _accountService.WithdrawAsync(accountNumber, request.Amount);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the transaction history for a specific account.
        /// </summary>
        /// <param name="accountNumber">The account number to fetch history for.</param>
        /// <returns>A list of past transactions.</returns>
        [HttpGet("{accountNumber}/transactions")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TransactionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactions(string accountNumber)
        {
            var transactions = await _accountService.GetTransactionHistoryAsync(accountNumber);

            var response = transactions.Select(t => new TransactionResponse(
                t.Id,
                t.Type.ToString(),
                t.Amount,
                t.BalanceAfter,
                t.CreatedAt
            ));

            return Ok(
                new ApiResponse<IEnumerable<TransactionResponse>>
                {
                    Success = true,
                    Data = response
                }
            );

        }
    }
}
