using FinTracker.Api.Common.Endpoints;
using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Api.Models;
using FinTracker.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region [Swagger Settings]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region [Authentication & Authorization]
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();
builder.Services.AddAuthorization();
#endregion

#region [Database Settings]
string connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});
#endregion

#region [Identity]
builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();
#endregion

#region [Dependency Injection]
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>()
                .AddTransient<ITransactionHandler, TransactionHandler>();
#endregion

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapEndpoints();

app.Run();
