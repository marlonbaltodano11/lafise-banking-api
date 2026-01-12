namespace BankingApi.Api.Contracts.Customers
{
    public record CreateCustomerRequest(
        string Name,
        DateTime BirthDate,
        string Gender,
        decimal Income
    );
}
