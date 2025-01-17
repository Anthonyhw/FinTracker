using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class CreateOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
                .WithName("Orders: Create Order")
                .WithSummary("Cria um pedido")
                .WithDescription("Cria um pedido")
                .WithOrder(5)
                .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(IOrderHandler handler, CreateOrderRequest request, ClaimsPrincipal user)
        {
            request.UserId = user.Identity!.Name ?? string.Empty;

            var result = await handler.CreateAsync(request);

            return result.IsSuccess ? 
                TypedResults.Created($"v1/orders/{result.Data?.Code}", result) :
                TypedResults.BadRequest(result);
        }
    }
}
