using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Core.Handlers
{
    public interface IProductHandler
    {
        Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request);
        Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request);
    }
}
