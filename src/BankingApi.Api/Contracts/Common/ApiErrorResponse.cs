using System.ComponentModel;

namespace BankingApi.Api.Contracts.Common
{
    public class ApiErrorResponse : ApiResponse<object>
    {
        public ApiErrorResponse()
        {
            Success = false;
            Data = null;
        }

        [DefaultValue(false)]
        public new bool Success { get; set; } = false;

        [DefaultValue(null)]
        public new object? Data { get; init; } = null;
    }
}
