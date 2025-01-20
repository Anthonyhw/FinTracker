using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.App.Handlers
{
    public class ProductHandler(IHttpClientFactory httpClientFactory) : IProductHandler
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
            => await _httpClient.GetFromJsonAsync<PagedResponse<List<Product>?>>("v1/products")
            ?? new PagedResponse<List<Product>?>(null, 400, "Não foi possível obter produtos.");

        public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
        => await _httpClient.GetFromJsonAsync<Response<Product?>>($"v1/products/{request.Slug}")
            ?? new Response<Product?>(null, 400, "Não foi possível obter o produto.");
    }
}
