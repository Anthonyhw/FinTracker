﻿using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Api.Models;
using FinTracker.Core;
using FinTracker.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FinTracker.Api.Common.Api
{
    public static class BuilderExtensions
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configuration.ConnectionString = builder.Configuration
                .GetConnectionString("DefaultConnection") ?? string.Empty;

            Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
            Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;

            ApiConfiguration.StripeApiKey = builder.Configuration.GetValue<string>("StripeApiKey") ?? string.Empty;
            StripeConfiguration.ApiKey = ApiConfiguration.StripeApiKey;
        }

        public static void AddDocumentation(this WebApplicationBuilder builder)
        {
            #region [Swagger Settings]
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion
        }

        public static void AddSecurity(this WebApplicationBuilder builder)
        {
            #region [Authentication & Authorization]
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                            .AddIdentityCookies();
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("PremiumAccess", policy =>
                {
                    policy.RequireClaim("PremiumUser", "true");
                });
            });
            #endregion
        }

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {
            #region [Database Settings]
            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.ConnectionString);
            });
            #endregion

            #region [Identity]
            builder.Services.AddIdentityCore<User>()
                            .AddRoles<IdentityRole<long>>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddApiEndpoints();
            #endregion
        }
        public static void AddCrossOrigin(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(
            opt => opt.AddPolicy(ApiConfiguration.CorsPolicyName,
            p => p.WithOrigins([
                         Configuration.BackendUrl,
                         Configuration.FrontendUrl
                  ])
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials()
            ));
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            #region [Dependency Injection]
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>()
                            .AddTransient<ITransactionHandler, TransactionHandler>()
                            .AddTransient<IReportHandler, ReportHandler>()
                            .AddTransient<IProductHandler, ProductHandler>()
                            .AddTransient<IVoucherHandler, VoucherHandler>()
                            .AddTransient<IOrderHandler, OrderHandler>()
                            .AddTransient<IStripeHandler, StripeHandler>()
                            ;
            #endregion
        }
    }
}