using FinTracker.Api.Common.Api;
using FinTracker.Api.Common.Endpoints.Categories;
using FinTracker.Api.Common.Endpoints.Transactions;

namespace FinTracker.Api.Common.Endpoints
{
    public static class Endpoint
    {
        public static void MapEndpoints(this WebApplication app)
        {
            var endpoints = app.MapGroup("");

            #region [HealtCheck]
            endpoints.MapGroup("")
                     .WithTags("HealthCheck")
                     .MapGet("/", () => new { message = "Ok!" })
                     .WithOrder(1);
            #endregion

            #region [Categories]
            endpoints.MapGroup("v1/categories")
                     .WithTags("Categories")
                     .MapEndpoint<GetCategoryByIdEndpoint>()
                     .MapEndpoint<GetAllCategoriesEndpoint>()
                     .MapEndpoint<CreateCategoryEndpoint>()
                     .MapEndpoint<UpdateCategoryEndpoint>()
                     .MapEndpoint<DeleteCategoryEndpoint>();
            #endregion

            #region [Transactions]
            endpoints.MapGroup("v1/transactions")
                     .WithTags("Transactions")
                     .MapEndpoint<GetTransactionByIdEndpoint>()
                     .MapEndpoint<GetTransactionsByPeriodEndpoint>()
                     .MapEndpoint<CreateTransactionEndpoint>()
                     .MapEndpoint<UpdateTransactionEndpoint>()
                     .MapEndpoint<DeleteTransactionEndpoint>();
            #endregion

        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
