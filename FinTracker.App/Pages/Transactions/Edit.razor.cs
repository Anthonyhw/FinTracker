using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Transactions
{
    public partial class EditTransactionPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        [Parameter]
        public string Id { get; set; } = string.Empty;
        public UpdateTransactionRequest InputModel { get; set; } = new UpdateTransactionRequest();
        public List<Category> Categories { get; set; } = new();

        #endregion

        #region Services

        [Inject]
        public ITransactionHandler TransactionHandler { get; set; } = null!;
        [Inject]
        public ICategoryHandler CategoryHandler { get; set; } = null!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            await GetTransactionByIdAsync();
            await GetCategoriesAsync();
            IsBusy = false;
        }
        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await TransactionHandler.UpdateAsync(InputModel);
                if (result.IsSuccess)
                {
                    NavigationManager.NavigateTo("/lancamentos/historico");
                }
                else
                {
                    Snackbar.Add(result.Message ?? "Erro ao processar operação.", Severity.Error);
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

        private async Task GetTransactionByIdAsync()
        {
            IsBusy = true;
            try
            {
                GetTransactionByIdRequest request = new() { Id = long.Parse(Id)};

                var response = await TransactionHandler.GetByIdAsync(request);
                if (response.IsSuccess && response.Data is not null)
                {
                    InputModel = new()
                    {
                        Id = response.Data.Id,
                        Amount = response.Data.Amount,
                        CategoryId = response.Data.CategoryId,
                        PaidOrReceivedAt = response.Data.PaidOrReceivedAt,
                        Title = response.Data.Title,
                        Type = response.Data.Type,
                    };
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            IsBusy = false;
        }

        private async Task GetCategoriesAsync()
        {
            IsBusy = true;
            try
            {
                GetAllCategoriesRequest request = new();

                var response = await CategoryHandler.GetAllAsync(request);
                if (response.IsSuccess && response.Data is not null)
                {
                    Categories = response.Data;
                    InputModel.CategoryId = Categories[0].Id;
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            IsBusy = false;
        }
        #endregion
    }
}