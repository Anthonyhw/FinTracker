using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Categories: Delete")
            .WithSummary("Exclui uma categoria.")
            .WithDescription("Exclui uma categoria.")
            .WithOrder(5)
            .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
        {
            var request = new DeleteCategoryRequest {UserId = "teste@hotmail.com", Id = id };
            var result = await handler.DeleteAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
