using FinTracker.Api.Common.Api;
using FinTracker.Api.Common.Endpoints.Categories;

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

        }

        private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
        {
            TEndpoint.Map(app);
            return app;
        }
    }
}
