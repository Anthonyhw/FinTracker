using FinTracker.Core.Handlers;
using FinTracker.Core.Requests;
using FinTracker.Core.Requests.Account;
using FinTracker.Core.Responses;
using System.Net.Http.Json;
using System.Text;

namespace FinTracker.App.Handlers
{
    public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<string>> LoginAsync(LoginRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
            return result.IsSuccessStatusCode ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!")
                    : new Response<string>("", 400, "Não foi possível realizar o login.");
        }

        public async Task LogoutAsync()
        {
            var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
            await _httpClient.PostAsJsonAsync("v1/identity/logout", emptyContent);
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/identity/register", request);
            return result.IsSuccessStatusCode ? new Response<string>("Cadastro realizado com sucesso!", 200, "Cadastro realizado com sucesso!")
                    : new Response<string>("", 400, "Não foi possível realizar o Cadastro.");
        }
    }
}
