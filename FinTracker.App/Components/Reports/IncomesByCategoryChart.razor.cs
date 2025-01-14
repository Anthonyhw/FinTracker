﻿using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Components.Reports
{
    public partial class IncomesByCategoryChartComponent : ComponentBase
    {
        #region Properties
        public List<double> Data { get; set; } = [];
        public List<string> Labels { get; set; } = [];
        #endregion

        #region Services
        [Inject]
        public IReportHandler Handler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            await GetIncomesByCategoryAsync();
        }

        private async Task GetIncomesByCategoryAsync()
        {
            var request = new GetIncomesByCategoryRequest();
            var result = await Handler.GetIncomesByCategoryReportAsync(request);

            if (!result.IsSuccess || result.Data is null)
            {
                Snackbar.Add("Falha ao obter dados de despesa.", Severity.Error);
                return;
            }

            foreach (var item in result.Data)
            {
                Labels.Add($"{item.Category} ({item.Incomes})");
                Data.Add((double)item.Incomes);
            }
        }
        #endregion
    }
}
