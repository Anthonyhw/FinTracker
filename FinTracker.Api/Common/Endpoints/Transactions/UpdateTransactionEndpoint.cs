﻿using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Common.Endpoints.Transactions
{
    public class UpdateTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Atualiza uma Transação.")
            .WithDescription("Atualiza uma Transação.")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(
            UpdateTransactionRequest request, ITransactionHandler handler, long id)
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