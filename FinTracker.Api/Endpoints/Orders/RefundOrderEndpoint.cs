using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class RefundOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/refund/{id}", HandleAsync)
            .WithName("Order: Refund Order")
            .WithSummary("Reembolsa um pedido")
            .WithDescription("Reembolsa um pedido")
            .WithOrder(8)
            .Produces<Response<Order?>>()
            ;

        private static async Task<IResult> HandleAsync(IOrderHandler handler, long id, ClaimsPrincipal user)
        {
            var request = new RefundOrderRequest()
            {
                Id = id,
                UserId = user.Identity!.Name ?? string.Empty
            };

            var result = await handler.RefundAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
