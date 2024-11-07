using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;
using System.Security.Claims;

namespace FinTracker.Api.Common.Endpoints.Transactions
{
    public class CreateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Transactions: Create")
            .WithSummary("Cria uma nova Transação.")
            .WithDescription("Cria uma nova Transação.")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user,
            CreateTransactionRequest request, ITransactionHandler handler)
        {
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.CreateAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Created($"/{result.Data?.Id}", result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
