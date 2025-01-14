using System.Net.Http.Json;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Models.Reports;
using FinTracker.Core.Requests.Reports;
using FinTracker.Core.Responses;

namespace FinTracker.App.Handlers
{
    public class ReportHandler(IHttpClientFactory httpClientFactory) : IReportHandler
    {

        private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
        public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<List<IncomesByCategory>?>>($"v1/reports/incomes");
            return result ?? new Response<List<IncomesByCategory>?>(null, 400, "Falha ao recuperar Entradas.");
        }
        
        public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>($"v1/reports/expenses");
            return result ?? new Response<List<ExpensesByCategory>?>(null, 400, "Falha ao recuperar Saídas.");
        }

        public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<FinancialSummary?>>($"v1/reports/summary");
            return result ?? new Response<FinancialSummary?> (null, 400, "Falha ao recuperar Resumo Financeiro.");
        }

        public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
        {
            var result = await _client.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>($"v1/reports/incomes-and-expenses");
            return result ?? new Response<List<IncomesAndExpenses>?>(null, 400, "Falha ao recuperar Entradas e Saídas.");
        }
    }
}
