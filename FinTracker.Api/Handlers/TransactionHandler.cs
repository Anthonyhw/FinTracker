using FinTracker.Api.Data;
using FinTracker.Core.Common.Extensions;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Api.Handlers
{
    public class TransactionHandler(AppDbContext _context) : ITransactionHandler
    {
        public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
        {
            try
            {
                var transaction = await _context.Transactions.AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);
                
                return transaction is null
                    ? new Response<Transaction?>(null, 404, "Transação não encontrada.")
                    : new Response<Transaction?>(transaction, 200);
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível obter a transação.");
            }
        }

        public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
        {
            try
            {
                request.StartDate ??= DateTime.Now.GetFirstDay();
                request.EndDate ??= DateTime.Now.GetLastDay();

                var query = _context.Transactions
                    .AsNoTracking()
                    .Where(t => t.UserId == request.UserId && t.CreatedAt >= request.StartDate && t.CreatedAt <= request.EndDate)
                    .OrderBy(t => t.CreatedAt);

                var transactions = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync();

                var count = transactions.Count();


                return transactions is null
                    ? new PagedResponse<List<Transaction>>(null, 404, "Transações não encontradas.")
                    : new PagedResponse<List<Transaction>>(transactions, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Transaction>>(null, 500, "Não foi possível obter as transações.");
            }
        }

        public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
        {
            try
            {
                var transaction = new Transaction
                {
                    UserId = request.UserId,
                    Title = request.Title,
                    Amount = (request.Type == EtransactionType.Withdraw && request.Amount > 0) ? request.Amount * -1 : request.Amount,
                    Type = request.Type,
                    CategoryId = request.CategoryId,
                    CreatedAt = DateTime.Now,
                    PaidOrReceivedAt = request.PaidOrReceivedAt
                };

                await _context.AddAsync<Transaction>(transaction);
                await _context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível criar a transação.");
            }
        }
        
        public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
        {
            try
            {
                var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);
                
                if (transaction is null)
                {
                    return new Response<Transaction?>(null, 404, "Transação não encontrada.");
                }

                transaction.Title = request.Title;
                transaction.Type = request.Type;
                transaction.Amount = (request.Type == EtransactionType.Withdraw && request.Amount > 0) ? request.Amount * -1 : request.Amount;
                transaction.CategoryId = request.CategoryId;
                transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
                
                return new Response<Transaction?>(transaction, 200, "Transação atualizada com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação.");
            }
        }

        public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
        {
            try
            {
                var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId);
                if (transaction is null)
                {
                    return new Response<Transaction?>(null, 404, "Transação não encontrada.");
                }

                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                return new Response<Transaction?>(transaction, 200, "Transação removida com sucesso!");
            }
            catch
            {
                return new Response<Transaction?>(null, 500, "Não foi possível remover a transação.");
            }
        }

    }
}
