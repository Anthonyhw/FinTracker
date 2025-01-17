﻿using FinTracker.Api.Data;
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
                    .FirstOrDefaultAsync(v => v.Code == request.Code && 
                                              v.IsActive);
                return voucher is null ? 
                    new Response<Voucher?>(null, 404, "Cupom não encontrado.") : 
                    new Response<Voucher?>(voucher);
            }
			catch
			{
                return new Response<Voucher?>(null, 500, "Não foi possível recuperar Cupom.");
            }
        }
    }
}
