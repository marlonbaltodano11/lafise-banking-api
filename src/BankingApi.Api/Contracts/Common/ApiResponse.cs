namespace BankingApi.Api.Contracts.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; init; }
        public T? Data { get; init; }
        public List<ApiError> Errors { get; init; } = [];
    }
}
