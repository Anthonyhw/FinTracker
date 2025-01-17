using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class GetVoucherByNumberEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/{number}", HandleAsync)
            .WithName("Voucher: Get By Number")
            .WithSummary("Retorna um cupom pelo código")
            .WithDescription("Retorna um cupom pelo código")
            .WithOrder(4)
            .Produces<Response<Voucher?>>();

        private static async Task<IResult> HandleAsync(IVoucherHandler handler, string Number)
        {
            var request = new GetVoucherByNumberRequest()
            {
                Code = Number
            };

            var result = await handler.GetByNumberAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
