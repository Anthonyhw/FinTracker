using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;
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

app.MapGet("/v1/categories", async
            (ICategoryHandler handler) =>
            {
                var request = new GetAllCategoriesRequest()
                {
                    UserId = "teste@hotmail.com"
                };
                return await handler.GetAllAsync(request);
            })
            .WithName("Categories: Get All")
            .WithSummary("Retorna todas as categorias do usuário.")
            .Produces<Response<List<Category>>>();

app.MapGet("/v1/categories/{id}", async
            (long id, ICategoryHandler handler) =>
            {
                var request = new GetCategoryByIdRequest()
                {
                    Id = id,
                    UserId = "teste@hotmail.com"
                };
                return await handler.GetByIdAsync(request);
            })  
            .WithName("Categories: Get By Id")
            .WithSummary("retorna uma categoria.")
            .Produces<Response<Category?>>();

app.MapPost("/v1/categories", async
            ([FromBody] CreateCategoryRequest request, ICategoryHandler handler) => await handler.CreateAsync(request))
            .WithName("Categories: Create")
            .WithSummary("Cria uma nova categoria.")
            .Produces<Response<Category?>>();

app.MapPut("/v1/categories/{id}", async
            (long id, [FromBody] UpdateCategoryRequest request, ICategoryHandler handler) =>
            {
                request.Id = id;
                return await handler.UpdateAsync(request);
            })
            .WithName("Categories: Update")
            .WithSummary("Atualiza uma categoria.")
            .Produces<Response<Category?>>();

app.MapDelete("/v1/categories/{id}", async
            (long id, ICategoryHandler handler) =>
            {
                var request = new DeleteCategoryRequest() { Id = id };
                return await handler.DeleteAsync(request);
            })
            .WithName("Categories: Delete")
            .WithSummary("Remove uma nova categoria.")
            .Produces<Response<Category?>>();

app.Run();
