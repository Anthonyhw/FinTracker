using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Categories
{
    public partial class ListCategoriesPage : ComponentBase
    {

        #region Properties
        public bool IsBusy { get; set; } = false;
        public List<Category> Categories { get; set; } = [];
        public string SearchFilter { get; set; } = string.Empty;
        #endregion

        #region Services

        [Inject]
        public ICategoryHandler Handler { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;

            try
            {
                var request = new GetAllCategoriesRequest();
                var result = await Handler.GetAllAsync(request);
                if (result.IsSuccess) 
                {
                    Categories = result.Data ?? [];
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
        #endregion

        #region Methods
        public Func<Category, bool> Filter => cat =>
        {
            if (string.IsNullOrEmpty(SearchFilter))
                return true;

            if (cat.Id.ToString().Contains(SearchFilter, StringComparison.OrdinalIgnoreCase)
                || cat.Title.ToString().Contains(SearchFilter, StringComparison.OrdinalIgnoreCase)
                || (cat.Description is not null && cat.Description.Contains(SearchFilter, StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        };

        public async void OnDeleteButtonClickedAsync(long id, string title)
        {
            var result = await DialogService.ShowMessageBox("Atenção", $"Deseja realmente excluir a categoria \"{title}\"?", yesText: "Confirmar", noText: "Cancelar");
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
                DeleteCategoryRequest request = new() { Id = id };
                await Handler.DeleteAsync(request);
                Categories.RemoveAll(x => x.Id == id);
                Snackbar.Add($"Categoria \"{title}\" removida com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
