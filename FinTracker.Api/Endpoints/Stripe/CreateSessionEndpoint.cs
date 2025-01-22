using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Stripe;

namespace FinTracker.Api.Endpoints.Stripe
{
    public class CreateSessionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/session", HandleAsync)
            .Produces<string?>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IStripeHandler handler, CreateSessionRequest request)
        {
            request.UserId = user.Identity.Name ?? string.Empty;

            var result = await handler.CreateSessionAsync(request);

            return result.IsSuccess ? 
                TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
