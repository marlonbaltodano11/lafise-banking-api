namespace BankingApi.Api.Contracts.Accounts
{
    public record CreateAccountResponse
    (
        string AccountNumber,
        decimal Balance
    );
}
