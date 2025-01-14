using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Responses;
using System.Security.Claims;

namespace FinTracker.Api.Endpoints.Reports
{
    public class GetIncomesByCategoryReportEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes", HandleAsync)
            .WithName("Reports: Get Incomes By Category")
            .WithSummary("Retorna entradas baseado em uma categoria.")
            .WithDescription("Retorna entradas baseado em uma categoria.")
            .WithOrder(1)
            .Produces<Response<List<IncomesByCategory>?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
        {
            var request = new GetIncomesByCategoryRequest();
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetIncomesByCategoryReportAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.StatusCode(result.Code);
        }
    }
}
