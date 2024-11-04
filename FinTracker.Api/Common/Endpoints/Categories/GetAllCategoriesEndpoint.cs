using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{pageNumber}/{pageSize}", HandleAsync)
            .WithName("Categories: Get All Categories")
            .WithSummary("Recupera todas as categorias")
            .WithDescription("Recupera todas as categorias")
            .WithOrder(1)
            .Produces<PagedResponse<List<Category?>>>();

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, int pageNumber, int pageSize)
        {
            var request = new GetAllCategoriesRequest()
            {
                UserId = "teste@hotmail.com",
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            var result = await handler.GetAllAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
