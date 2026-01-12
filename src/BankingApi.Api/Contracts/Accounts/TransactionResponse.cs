namespace BankingApi.Api.Contracts.Accounts
{
    public record TransactionResponse
    (
        Guid Id,
        string Type,
        decimal Amount,
        decimal BalanceAfter,
        DateTime CreatedAt
    );
}
