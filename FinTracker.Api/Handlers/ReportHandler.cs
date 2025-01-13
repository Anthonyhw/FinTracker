using FinTracker.Api.Data;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Api.Handlers
{
    public class ReportHandler(AppDbContext _context) : IReportHandler
    {
        public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
        {
            try
            {
                var data = await _context.ExpensesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();

                return new Response<List<ExpensesByCategory>?>(data);
            }
            catch
            {
                return new Response<List<ExpensesByCategory>?>(null, 500, "Não foi possível obter as saídas.");
            }
        }

        public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
        {
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var data = await _context.Transactions
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId 
                            && x.PaidOrReceivedAt >= startDate
                            && x.PaidOrReceivedAt <= DateTime.Now)
                    .GroupBy(x => 1)
                    .Select(x => new FinancialSummary(request.UserId, 
                                                        x.Where(t => t.Type == EtransactionType.Deposit).Sum(t => t.Amount),
                                                        x.Where(t => t.Type == EtransactionType.Withdraw).Sum(t => t.Amount)))
                    .FirstOrDefaultAsync();

                return new Response<FinancialSummary?>(data);
            }
            catch
            {
                return new Response<FinancialSummary?>(null, 500, "Não foi possível obter o resumo financeiro.");
            }
        }

        public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
        {
            try
            {
                var data = await _context.IncomesAndExpenses
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

                return new Response<List<IncomesAndExpenses>?>(data);
            }
            catch
            {
                return new Response<List<IncomesAndExpenses>?>(null, 500, "Não foi possível obter as entradas e saídas.");
            }
        }

        public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
        {
            try
            {
                var data = await _context.IncomesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();

                return new Response<List<IncomesByCategory>?>(data);
            }
            catch
            {
                return new Response<List<IncomesByCategory>?>(null, 500, "Não foi possível obter as entradas.");
            }
        }
    }
}
