using FinTracker.Api.Common.Api;
using FinTracker.Api.Endpoints.Categories;
using FinTracker.Api.Endpoints.Identity;
using FinTracker.Api.Endpoints.Orders;
using FinTracker.Api.Endpoints.Reports;
using FinTracker.Api.Endpoints.Stripe;
using FinTracker.Api.Endpoints.Transactions;
using FinTracker.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinTracker.Api.Endpoints
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

            #region [Identity]
            endpoints.MapGroup("v1/identity")
                     .WithTags("Identity")
                     .MapIdentityApi<User>();

            endpoints.MapGroup("v1/identity")
                     .WithTags("Identity")
                     .RequireAuthorization()
                     .MapEndpoint<LogoutEndpoint>()
                     .MapEndpoint<GetRolesEndpoint>()
                     .MapEndpoint<GetClaimsEndpoint>();
            #endregion

            #region [Categories]
            endpoints.MapGroup("v1/categories")
                     .WithTags("Categories")
                     .RequireAuthorization()
                     .MapEndpoint<GetCategoryByIdEndpoint>()
                     .MapEndpoint<GetAllCategoriesEndpoint>()
                     .MapEndpoint<CreateCategoryEndpoint>()
                     .MapEndpoint<UpdateCategoryEndpoint>()
                     .MapEndpoint<DeleteCategoryEndpoint>();
            #endregion

            #region [Transactions]
            endpoints.MapGroup("v1/transactions")
                     .WithTags("Transactions")
                     .RequireAuthorization()
                     .MapEndpoint<GetTransactionByIdEndpoint>()
                     .MapEndpoint<GetTransactionsByPeriodEndpoint>()
                     .MapEndpoint<CreateTransactionEndpoint>()
                     .MapEndpoint<UpdateTransactionEndpoint>()
                     .MapEndpoint<DeleteTransactionEndpoint>();
            #endregion

            #region [Reports]
            endpoints.MapGroup("v1/reports")
                     .WithTags("Reports")
                     .RequireAuthorization()
                     .MapEndpoint<GetExpensesByCategoryReportEndpoint>()
                     .MapEndpoint<GetIncomesByCategoryReportEndpoint>()
                     .MapEndpoint<GetFinancialSummaryReportEndpoint>()
                     .MapEndpoint<GetIncomesAndExpensesReportEndpoint>();
            #endregion

            #region [Orders]
            endpoints.MapGroup("v1/orders")
                     .WithTags("Orders")
                     .RequireAuthorization()
                     .MapEndpoint<GetAllOrdersEndpoint>()
                     .MapEndpoint<GetOrderByNumberEndpoint>()
                     .MapEndpoint<CreateOrderEndpoint>()
                     .MapEndpoint<CancelOrderEndpoint>()
                     .MapEndpoint<PayOrderEndpoint>()
                     .MapEndpoint<RefundOrderEndpoint>();
            #endregion

            #region [Products]
            endpoints.MapGroup("v1/products")
                     .WithTags("Products")
                     .RequireAuthorization()
                     .MapEndpoint<GetAllProductsEndpoint>()
                     .MapEndpoint<GetProductBySlugEndpoint>();
            #endregion

            #region [Vouchers]
            endpoints.MapGroup("v1/vouchers")
                     .WithTags("Vouchers")
                     .RequireAuthorization()
                     .MapEndpoint<GetVoucherByNumberEndpoint>();
            #endregion

            #region [Stripe]
            endpoints.MapGroup("v1/payments/stripe")
                     .WithTags("Payments - Stripe")
                     .RequireAuthorization()
                     .MapEndpoint<CreateSessionEndpoint>();
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
