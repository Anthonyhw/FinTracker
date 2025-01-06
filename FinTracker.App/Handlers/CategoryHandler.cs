using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;

namespace FinTracker.App.Handlers
{
    public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
    {

        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            var result = await _client.PostAsJsonAsync("v1/categories", request);
            return await result.Content.ReadFromJsonAsync<Response<Category?>>() 
                ?? new Response<Category?>(null, 400, "Falha ao criar categoria.");
        }

        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            var result = await _client.DeleteAsync($"v1/categories/{request.Id}");
            return await result.Content.ReadFromJsonAsync<Response<Category?>>()
                ?? new Response<Category?>(null, 400, "Falha ao excluir categoria.");
        }

        public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            var result = await _client.GetFromJsonAsync<PagedResponse<List<Category>>>($"v1/categories");
            return result ?? new PagedResponse<List<Category>>(null, 400, "Falha ao recuperar categorias.");
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}");
            return result ?? new Response<Category?>(null, 400, "Falha ao recuperar categoria.");
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            var result = await _client.PutAsJsonAsync($"v1/categories/{request.Id}", request);
            return await result.Content.ReadFromJsonAsync<Response<Category?>>()
                ?? new Response<Category?>(null, 400, "Falha ao atualizar categoria.");
        }
    }
}
