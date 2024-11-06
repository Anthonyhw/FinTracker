﻿using FinTracker.Api.Common.Api;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;

namespace FinTracker.Api.Common.Endpoints.Transactions
{
    public class DeleteTransactionEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transactions: Delete")
            .WithSummary("Remove uma Transação.")
            .WithDescription("Remove uma Transação.")
            .WithOrder(5)
            .Produces<Response<Transaction?>>();

        private static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
        {
            var request = new DeleteTransactionRequest { Id = id, UserId = "teste@hotmail.com" };

            var result = await handler.DeleteAsync(request);
            
            return result.IsSuccess 
                ? TypedResults.Ok(result) 
                : TypedResults.StatusCode(result.Code);
        }
    }
}