using FinTracker.Core.Common.Extensions;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Transactions
{
    public class ListTransactionPage : ComponentBase
    {


        #region Properties
        public bool IsBusy { get; set; } = false;
        public List<Transaction> Transactions { get; set; } = [];
        public string SearchFilter { get; set; } = string.Empty;
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public int CurrentMonth { get; set; } = DateTime.Now.Month;
        public int[] Years { get; set; } =
        {
            DateTime.Now.Year,
            DateTime.Now.AddYears(-1).Year,
            DateTime.Now.AddYears(-2).Year,
            DateTime.Now.AddYears(-3).Year,
            DateTime.Now.AddYears(-4).Year,
            DateTime.Now.AddYears(-5).Year,
        };
        #endregion

        #region Services

        [Inject]
        public ITransactionHandler Handler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync() => await GetTransactions();
        #endregion

        #region Methods
        public Func<Transaction, bool> Filter => trans =>
        {
            if (string.IsNullOrEmpty(SearchFilter))
                return true;

            if (trans.Id.ToString().Contains(SearchFilter, StringComparison.OrdinalIgnoreCase)
                || trans.Title.ToString().Contains(SearchFilter, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private async Task GetTransactions()
        {
            IsBusy = true;

            try
            {
                var request = new GetTransactionsByPeriodRequest()
                {
                    StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                    EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                    PageNumber = 1,
                    PageSize = 10,
                };
                var result = await Handler.GetByPeriodAsync(request);
                if (result.IsSuccess)
                {
                    Transactions = result.Data ?? [];
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnDeleteButtonClickedAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("Atenção", $"Deseja realmente excluir a transação \"{title}\"?", yesText: "Confirmar", noText: "Cancelar");
            if (result is true)
            {
                await OnDeleteAsync(id, title);
                StateHasChanged();
            }
        }

        public async Task OnDeleteAsync(long id, string title)
        {
            try
            {
                DeleteTransactionRequest request = new() { Id = id };
                await Handler.DeleteAsync(request);
                Transactions.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Lançamento \"{title}\" removido com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task OnSearchAsync()
        {
            await GetTransactions();
            StateHasChanged();
        }
        #endregion
    }
}
