using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Categories
{
    public class GetCategoryByIdEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: Get By Id")
            .WithSummary("Recupera uma categoria")
            .WithDescription("Recupera uma categoria")
            .WithOrder(2)
            .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
        {
            var request = new GetCategoryByIdRequest {UserId = "teste@hotmail.com", Id = id };
            var result = await handler.GetByIdAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
