using System.Security.Claims;
using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Endpoints.Reports
{
    public class GetFinancialSummaryReportEndpoint: IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/summary", HandleAsync)
            .WithName("Reports: Get Financial Summary")
            .WithSummary("Retorna o resumo financeiro do usuário.")
            .WithDescription("Retorna o resumo financeiro do usuário.")
            .WithOrder(2)
            .Produces<Response<FinancialSummary?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
        {
            var request = new GetFinancialSummaryRequest();
            request.UserId = user.Identity?.Name ?? string.Empty;
            var result = await handler.GetFinancialSummaryReportAsync(request);

            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.StatusCode(result.Code);
        }
    }
}
