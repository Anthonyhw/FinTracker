using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using FinTracker.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Transactions
{
    public partial class CreateTransactionPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public CreateTransactionRequest InputModel { get; set; } = new CreateTransactionRequest();
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

        }
        #endregion

        #region Methods

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;

            try
            {
                var result = await TransactionHandler.CreateAsync(InputModel);
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
            #endregion
        }   
    }
}