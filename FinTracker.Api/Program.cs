using FinTracker.Api.Common.Endpoints;
using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Core.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region [Database Settings]
string connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(connectionString);
});
#endregion

#region [Swagger Settings]
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region [Dependency Injection]
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
#endregion

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "Ok!" });
app.MapEndpoints();

app.Run();
