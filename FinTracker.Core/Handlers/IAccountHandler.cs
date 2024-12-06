using FinTracker.Core.Requests.Account;
using FinTracker.Core.Responses;

namespace FinTracker.Core.Handlers
{
    public interface IAccountHandler
    {
        Task<Response<string>> LoginAsync(LoginRequest request);
        Task<Response<string>> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();
    }
}
