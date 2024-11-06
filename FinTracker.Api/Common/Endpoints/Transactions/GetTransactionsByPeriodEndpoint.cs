using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Transactions
{
    public class GetTransactionsByPeriodEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get Transactions By Period")
            .WithSummary("Recupera Transações baseado em um período.")
            .WithDescription("Recupera Transações baseado em um período.")
            .WithOrder(2)
            .Produces<PagedResponse<List<Transaction?>>>();

        private static async Task<IResult> HandleAsync(ITransactionHandler handler, 
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var request = new GetTransactionsByPeriodRequest
            {
                UserId = "teste@hotmail.com",
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartDate = startDate,
                EndDate = endDate
            };
            
            var result = await handler.GetByPeriodAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}
