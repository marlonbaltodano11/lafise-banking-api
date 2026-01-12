using BankingApi.Api.Contracts.Accounts;
using BankingApi.Application.Services;
using BankingApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using BankingApi.Api.Contracts.Common;
using Microsoft.AspNetCore.Components.Forms.Mapping;

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

        [HttpPost]
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

        [HttpGet("{accountNumber}/balance")]
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

        [HttpPost("{accountNumber}/deposit")]
        public async Task<IActionResult> Deposit(string accountNumber, AmountRequest request)
        {
            await _accountService.DepositAsync(accountNumber, request.Amount);
            return NoContent();
        }

        [HttpPost("{accountNumber}/withdraw")]
        public async Task<IActionResult> Withdraw(string accountNumber, AmountRequest request)
        {
            await _accountService.WithdrawAsync(accountNumber, request.Amount);
            return NoContent();
        }

        [HttpGet("{accountNumber}/transactions")]
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
