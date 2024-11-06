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

            #region [Categories]
            endpoints.MapGroup("v1/categories")
                     .WithTags("Categories")
                     .MapEndpoint<CreateCategoryEndpoint>()
                     .MapEndpoint<UpdateCategoryEndpoint>()
                     .MapEndpoint<DeleteCategoryEndpoint>()
                     .MapEndpoint<GetCategoryByIdEndpoint>()
                     .MapEndpoint<GetAllCategoriesEndpoint>();
            #endregion

            #region [Transactions]
            endpoints.MapGroup("v1/transactions")
                     .WithTags("Transactions")
                     .MapEndpoint<CreateTransactionEndpoint>()
                     .MapEndpoint<UpdateTransactionEndpoint>()
                     .MapEndpoint<DeleteTransactionEndpoint>()
                     .MapEndpoint<GetTransactionByIdEndpoint>()
                     .MapEndpoint<GetTransactionsByPeriodEndpoint>();
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
