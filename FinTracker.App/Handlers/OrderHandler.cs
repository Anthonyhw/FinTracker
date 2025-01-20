using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.App.Handlers
{
    public class OrderHandler(IHttpClientFactory httpClientFactory) : IOrderHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"v1/orders/cancel/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Order?>>()
                ?? new Response<Order?>(null, 400, "Não foi possível cancelar o pedido.");
        }

        public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("v1/orders", request);
            return await result.Content.ReadFromJsonAsync<Response<Order?>>() 
                ?? new Response<Order?>(null, 400, "Não foi possível criar o pedido.");
        }

        public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
            => await _httpClient.GetFromJsonAsync<PagedResponse<List<Order>?>>("v1/orders")
            ?? new PagedResponse<List<Order>?>(null, 400, "Não foi possível recuperar pedidos.");

        public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
            => await _httpClient.GetFromJsonAsync<Response<Order?>>($"v1/orders/{request.Number}")
            ?? new Response<Order?>(null, 400, "Não foi possível recuperar o pedidos.");

        public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"v1/orders/pay/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Order?>>()
                ?? new Response<Order?>(null, 400, "Não foi possível efetuar o pagamento.");
        }

        public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync($"v1/orders/refund/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Order?>>()
                ?? new Response<Order?>(null, 400, "Não foi possível reembolsar o pagamento.");
        }
    }
}
