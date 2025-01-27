using FinTracker.Api.Data;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Api.Handlers
{
    public class VoucherHandler(AppDbContext _context) : IVoucherHandler
    {
        public async Task<Response<Voucher?>> GetByNumberAsync(GetVoucherByNumberRequest request)
        {
			try
			{
                var voucher = await _context.Vouchers.AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Code == request.Code);

                if (voucher is null)
                {
                    return new Response<Voucher?>(null, 404, "Cupom não encontrado.");
                }
                else if (!voucher.IsActive)
                {
                    return new Response<Voucher?>(null, 400, "Cupom inativo.");
                }
                else
                {
                    return new Response<Voucher?>(voucher);
                }
            }
			catch
			{
                return new Response<Voucher?>(null, 500, "Não foi possível recuperar Cupom.");
            }
        }
    }
}
