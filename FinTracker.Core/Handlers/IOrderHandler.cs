using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;

namespace FinTracker.Core.Handlers
{
    public interface IOrderHandler
    {
        Task<Response<Order?>> CancelAsync(CancelOrderRequest request);
        Task<Response<Order?>> CreateAsync(CreateOrderRequest request);
        Task<Response<Order?>> PayAsync(PayOrderRequest request);
        Task<Response<Order?>> RefundAsync(RefundOrderRequest request);
        Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request);
        Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request);
    }
}
