using System.Diagnostics;
using FinTracker.Api.Data;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Requests.Stripe;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Api.Handlers
{
    public class OrderHandler(AppDbContext _context, IStripeHandler _stripeHandler) : IOrderHandler
    {
        public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
        {
            Order? order;

            try
            {
                order = await _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

                if (order is null)
                    return new Response<Order?>(null, 404, "Pedido não encontrado.");

            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível recuperar Pedido.");
            }

            switch (order.Status)
            {
                case EOrderStatus.Canceled:
                    return new Response<Order?>(order, 400, "Este pedido já foi cancelado.");
                case EOrderStatus.WaitingPayment:
                    break;
                case EOrderStatus.Paid:
                    return new Response<Order?>(order, 400, "Este pedido já foi pago e não pode ser cancelado.");
                case EOrderStatus.Refunded:
                    return new Response<Order?>(order, 400, "Este pedido já foi reembolsado.");
                default:
                    return new Response<Order?>(order, 400, "Este pedido não pode ser cancelado.");
            }

            order.Status = EOrderStatus.Canceled;
            order.UpdatedAt = DateTime.Now;

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(order, 500, "Não foi possível cancelar o Pedido.");
            }

            return new Response<Order?>(order, 200, "Pedido cancelado com sucesso.");
        }

        public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
        {
            Product? product;
            try
            {
                product = await _context.Products.AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive);

                if (product is null)
                    return new Response<Order?>(null, 400, "Produto não encontrado.");

                // Evita que o EntityFramework recrie o objeto no banco caso ele seja atrelado a outro objeto.
                _context.Attach(product);
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível obter o produto.");
            }

            Voucher? voucher = null;
            try
            {
                if (request.VoucherId is not null)
                {
                    voucher = await _context.Vouchers.AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == request.VoucherId && v.IsActive);

                    if (voucher is null)
                        return new Response<Order?>(null, 400, "Cupom inválido ou não encontrado.");

                    voucher.IsActive = false;
                    _context.Vouchers.Update(voucher);
                }
            }
            catch 
            {
                return new Response<Order?>(null, 500, "Não foi possível obter o Cupom."); ;
            }

            Order order = new Order()
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Product = product,
                VoucherId = request.VoucherId,
                Voucher = voucher
            };

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível realizar o pedido.");
            }
            
            return new Response<Order?>(order, 201, $"Pedido {order.Code} realizado com sucesso.");
        }

        public async Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
        {
            try
            {
                var query = _context.Orders.AsNoTracking()
                    .Include(o => o.Product)
                    .Include(o => o.Voucher)
                    .Where(o => o.UserId == request.UserId)
                    .OrderByDescending(o => o.CreatedAt);

                var orders = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var count = orders.Count();

                return new PagedResponse<List<Order>?>(orders, count, request.PageNumber, request.PageSize);
            }
            catch
            {
                return new PagedResponse<List<Order>?>(null, 500, "Não foi possível recuperar pedidos.");
            }
        }

        public async Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
        {
            try
            {
                var order = await _context.Orders.AsNoTracking()
                    .Include(o => o.Product)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.UserId == request.UserId && o.Code == request.Number);

                return order is null ? 
                    new Response<Order?>(null, 404, "Pedido não encontrado.") :
                    new Response<Order?>(order);
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível recuperar o pedido.");
            }
        }

        public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
        {
            Order? order;

            try
            {
                order = await _context.Orders
                            .Include(o => o.Product)
                            .Include(o => o.Voucher)
                            .FirstOrDefaultAsync(o => o.UserId == request.UserId && o.Code == request.Number);

                if (order is null)
                    return new Response<Order?>(null, 404, "Pedido não encontrado.");
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível recuperar o Pedido.");
            }

            switch (order.Status)
            {
                case EOrderStatus.Canceled:
                    return new Response<Order?>(order, 400, "Este pedido foi cancelado e não pode ser pago.");
                case EOrderStatus.WaitingPayment:
                    break;
                case EOrderStatus.Paid:
                    return new Response<Order?>(order, 400, "Este pedido já está pago.");
                case EOrderStatus.Refunded:
                    return new Response<Order?>(order, 400, "Este pedido foi reembolsado e não pode ser pago.");
                default:
                    return new Response<Order?>(order, 400, "Não foi possível prosseguir com o pagamento.");
            }

            try
            {
                var getTransactionsRequest = new GetTransactionsByOrderNumberRequest
                {
                    Number = order.Code
                };
                
                var result = await _stripeHandler.GetTransactionsByOrderNumberAsync(getTransactionsRequest);

                if (!result.IsSuccess || result.Data is null)
                    return new Response<Order?>(null, 500, "Não foi possível localizar pagamento.");

                if (result.Data.Any(x => x.Refunded))
                    return new Response<Order?>(null, 400, "Este pedido já teve o pagamento reembolsado.");

                if (!result.Data.Any(x => x.Paid))
                    return new Response<Order?>(null, 400, "Este pedido não foi pago.");

                request.ExternalReference = result.Data[0].Id;

            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível dar baixa em seu pedido.");
            }

            order.Status = EOrderStatus.Paid;
            order.ExternalReference = request.ExternalReference;
            order.UpdatedAt = DateTime.Now;

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível prosseguir com o pagamento.");
            }

            return new Response<Order?>(order, 200, "Pedido pago com sucesso!");
        }

        public async Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
        {
            Order? order;
            
            try
            {
                order = await _context.Orders
                    .Include(o => o.Product)
                    .Include(o => o.Voucher)
                    .FirstOrDefaultAsync(o => o.Id == request.Id && o.UserId == request.UserId);

                if (order is null)
                    return new Response<Order?>(null, 404, "Pedido não encontrado.");
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível recuperar o pedido");
            }

            switch (order.Status)
            {
                case EOrderStatus.Canceled:
                    return new Response<Order?>(order, 400, "Este pedido foi cancelado e não pode ser reembolsado.");
                case EOrderStatus.WaitingPayment:
                    return new Response<Order?>(order, 400, "Este pedido não foi pago e não pode ser reembolsado.");
                case EOrderStatus.Paid:
                    break;
                case EOrderStatus.Refunded:
                    return new Response<Order?>(order, 400, "Este pedido já foi reembolsado.");
                default:
                    return new Response<Order?>(order, 400, "Não foi possível prosseguir com o reembolso.");
            }

            order.Status = EOrderStatus.Refunded;
            order.UpdatedAt = DateTime.Now;

            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(order, 500, "Falha ao reembolsar pagamento.");
            }

            return new Response<Order?>(order, 200, "Pedido reembolsado com sucesso.");
        }
    }
}
