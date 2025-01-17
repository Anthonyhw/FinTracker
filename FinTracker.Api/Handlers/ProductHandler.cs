using FinTracker.Api.Data;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Api.Handlers
{
    public class ProductHandler(AppDbContext _context) : IProductHandler
    {
        public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
        {
            try
            {
                var query = _context.Products.AsNoTracking()
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.Title);

                var products = await query
                    .Skip((request.PageNumber -1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = products.Count();

                return new PagedResponse<List<Product>?>(products, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Product>?>(null, 500, "Não foi possível recuperar produtos.");
            }
        }

        public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
        {
            try
            {
                var product = await _context.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IsActive && p.Slug == request.Slug);

                return product is null ? 
                    new Response<Product?>(null, 404, "Produto não encontrado.") :
                    new Response<Product?>(product);
            }
            catch
            {
                return new Response<Product?>(null, 500, "Não foi possível recuperar o produto.");
            }
        }
    }
}
