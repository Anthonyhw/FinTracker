using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.App.Handlers
{
    public class VoucherHandler(IHttpClientFactory httpClientFactory) : IVoucherHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
            => await _httpClient.GetFromJsonAsync<Response<Voucher?>>($"v1/vouchers/{request.Code}")
                ?? new Response<Voucher?>(null, 400, "Não foi possível obter o voucher.");
    }
}
