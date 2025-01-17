using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Core.Handlers
{
    public interface IVoucherHandler
    {
        Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request);
    }
}
