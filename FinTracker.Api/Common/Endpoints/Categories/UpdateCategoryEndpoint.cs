﻿using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinTracker.Api.Common.Endpoints.Categories
{
    public class UpdateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Categories: Update")
            .WithSummary("Atualiza uma categoria.")
            .WithDescription("Atualiza uma categoria.")
            .WithOrder(4)
            .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            UpdateCategoryRequest request, ICategoryHandler handler, long id)
        {
            request.UserId = "teste@hotmail.com";
            request.Id = id;
            var result = await handler.UpdateAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result)
                : TypedResults.StatusCode(result.Code);
        }
    }
}