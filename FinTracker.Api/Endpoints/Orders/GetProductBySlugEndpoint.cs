using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Orders
{
    public class GetProductBySlugEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
         => app.MapGet("/{slug}", HandleAsync)
            .WithName("Product: Get Product by Slug")
            .WithSummary("Recupera um produto pelo slug")
            .WithDescription("Recupera um produto pelo slug")
            .WithOrder(3)
            .Produces<Response<Product?>>()            ;

        private static async Task<IResult> HandleAsync(IProductHandler handler, string slug )
        {
            var request = new GetProductBySlugRequest()
            {
                Slug = slug
            };

            var result = await handler.GetBySlugAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result) :
                TypedResults.BadRequest(result);
        }
    }
}
