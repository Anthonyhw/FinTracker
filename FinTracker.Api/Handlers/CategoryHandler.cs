using FinTracker.Api.Data;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinTracker.Api.Handlers
{
    public class CategoryHandler(AppDbContext _context) : ICategoryHandler
    {
        public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
        {
            try
            {
                var query = _context.Categories
                    .AsNoTracking()
                    .Where(c => c.UserId == request.UserId)
                    .OrderBy(c => c.Title);

                var categories = await query.Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = await query.CountAsync();

                return categories.Count == 0
                ? new PagedResponse<List<Category>>(null, 404, "Categorias não encontradas.")
                : new PagedResponse<List<Category>>(categories, count, request.PageNumber, request.PageSize);
            }
            catch 
            {
                return new PagedResponse<List<Category>>(null, 500, "Não foi possível encontrar categorias.");
            }
        }

        public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        {
            try
            {
                var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                return category is null 
                    ? new Response<Category?>(null, 404, "Categoria não encontrada.") 
                    : new Response<Category?>(category);
            }
            catch
            {
                return new Response<Category?>(null, 500, "Não foi possível encontrar categoria.");
            }
        }

        public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
        {
            try
            {
                Category category = new Category()
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Description = request.Description,
                };

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return new Response<Category?>(category, 201, "Categoria criada com sucesso!");
            }
            catch
            {
                return new Response<Category?>(null, 500, "Não foi possível criar a categoria.");
            }
        }

        public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                if (category is null)
                {
                    return new Response<Category?>(null, 404, "Categoria não encontrada.");
                }

                category.Title = request.Title;
                category.Description = request.Description;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return new Response<Category?>(category);
            }
            catch 
            {
                return new Response<Category?>(null, 500, "Não foi possível atualizar categoria.");
            }
        }
        
        public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == request.UserId);

                if (category is null)
                {
                    return new Response<Category?>(null, 404, "Categoria não encontrada.");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new Response<Category?>(category);
            }
            catch
            {
                return new Response<Category?>(null, 500, "Não foi possível remover a categoria.");
            }
        }
    }
}
