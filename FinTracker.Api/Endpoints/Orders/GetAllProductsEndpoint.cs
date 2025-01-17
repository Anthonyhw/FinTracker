using FinTracker.Api.Common.Api;
using FinTracker.Core;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTracker.Api.Endpoints.Orders
{
    public class GetAllProductsEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Products: Get All Products")
            .WithSummary("Recupera todos os produtos")
            .WithDescription("Recupera todos os produtos")
            .WithOrder(1)
            .Produces<PagedResponse<List<Product?>>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IProductHandler handler, 
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber, [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllProductsRequest()
            {
                UserId = user.Identity?.Name ?? string.Empty,
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
