using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Endpoints.Orders
{
    public class GetAllOrdersEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
            .WithName("Orders: Get All Orders")
            .WithSummary("Retorna todos os pedidos do usuário.")
            .WithDescription("Retorna todos os pedidos do usuário.")
            .WithOrder(0)
            .Produces<PagedResponse<List<Order>?>>();

        private static async Task<IResult> HandleAsync(IOrderHandler handler, ClaimsPrincipal user, 
            [FromQuery] int pageNumber = Configuration.DefaultPageNumber, [FromQuery] int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllOrdersRequest()
            {
                UserId = user.Identity!.Name ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await handler.GetAllAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
