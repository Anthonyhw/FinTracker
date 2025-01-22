using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Stripe;
using FinTracker.Core.Responses;
using FinTracker.Core.Responses.Stripe;

namespace FinTracker.App.Handlers
{
    public class StripeHandler(IHttpClientFactory httpClientFactory) : IStripeHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<string?>> CreateSessionAsync(CreateSessionRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/payments/stripe/session", request);
            return await result.Content.ReadFromJsonAsync<Response<string?>>() ??
                new Response<string?>(null, 400, "Falha ao criar sessão no Stripe.");
        }

        public async Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"v1/payments/stripe/{request.Number}", request);
            return await result.Content.ReadFromJsonAsync<Response<List<StripeTransactionResponse>>>() ??
                new Response<List<StripeTransactionResponse>>(null, 400, "Falha ao consultar transações do pedido.");
        }
    }
}
