using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Reports
{
    public class GetExpensesByCategoryReportEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses", HandleAsync)
            .WithName("Reports: Get Expenses By Category")
            .WithSummary("Retorna despesas baseado em uma categoria.")
            .WithDescription("Retorna despesas baseado em uma categoria.")
            .WithOrder(0)
            .Produces<Response<List<ExpensesByCategory>?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
        {
            var request = new GetExpensesByCategoryRequest();
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetExpensesByCategoryReportAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.StatusCode(result.Code);
        }
    }
}
