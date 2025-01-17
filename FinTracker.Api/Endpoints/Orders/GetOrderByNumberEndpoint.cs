using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Api.Handlers;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class GetOrderByNumberEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{id}", HandleAsync)
            .WithName("Order: Get Order by Number")
            .WithSummary("Recupera um pedido pelo código.")
            .WithDescription("Recupera um pedido pelo código.")
            .WithOrder(2)
            .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(IOrderHandler handler, string code, ClaimsPrincipal user)
        {
            var request = new GetOrderByNumberRequest()
            {
                UserId = user.Identity!.Name ?? string.Empty,
                Number = code
            };

            var result = await handler.GetByNumberAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
