using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Categories
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Cria uma nova categoria.")
            .WithDescription("Cria uma nova categoria.")
            .WithOrder(3)
            .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            CreateCategoryRequest request, ICategoryHandler handler)
        {
            var result = await handler.CreateAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Created($"/{result.Data?.Id}", result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
