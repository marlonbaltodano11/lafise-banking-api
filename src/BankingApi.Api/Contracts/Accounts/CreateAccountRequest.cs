namespace BankingApi.Api.Contracts.Accounts
{
    public record CreateAccountRequest(
        Guid CustomerId,
        decimal InitialBalance
    );
}
