using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class PayOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/pay/{number}", HandleAsync)
            .WithName("Order: Pay Order")
            .WithSummary("Realiza o pagamento de um pedido")
            .WithDescription("Realiza o pagamento de um pedido")
            .WithOrder(7)
            .Produces<Response<Order>>();

        private static async Task<IResult> HandleAsync(IOrderHandler handler, string number, ClaimsPrincipal user, PayOrderRequest request)
        {
            request.UserId = user.Identity!.Name ?? string.Empty;
            request.Number = number;

            var result = await handler.PayAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
