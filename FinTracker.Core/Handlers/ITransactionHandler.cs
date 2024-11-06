using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Core.Responses;

namespace FinTracker.Core.Handlers
{
    public interface ITransactionHandler
    {
        Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request);
        Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request);
        Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request);
        Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request);
        Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request);
    }
}
