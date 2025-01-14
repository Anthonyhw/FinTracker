using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Responses;
using System.Security.Claims;

namespace FinTracker.Api.Endpoints.Reports
{
    public class GetIncomesAndExpensesReportEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-and-expenses", HandleAsync)
            .WithName("Reports: Get Incomes and Expenses By Category")
            .WithSummary("Retorna as receitas e despesas do usuário.")
            .WithDescription("Retorna as receitas e despesas do usuário.")
            .WithOrder(3)
            .Produces<Response<List<IncomesAndExpenses>?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
        {
            var request = new GetIncomesAndExpensesRequest();
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetIncomesAndExpensesReportAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.StatusCode(result.Code);
        }
    }
}
