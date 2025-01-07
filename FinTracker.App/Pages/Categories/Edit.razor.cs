using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Categories
{
    public partial class EditCategoryPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public UpdateCategoryRequest InputModel { get; set; } = new();

        #endregion

        #region Parameters
        [Parameter]
        public string Id { get; set; } = string.Empty;
        #endregion

        #region Services
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        ICategoryHandler Handler { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            GetCategoryByIdRequest? request = null!;
            
            try
            {
                request = new GetCategoryByIdRequest() 
                { 
                    Id = long.Parse(Id)
                };
            }
            catch (Exception)
            {
                Snackbar.Add("Parâmetro Inválido", Severity.Error);
            }

            IsBusy = true;
            try
            {
                var response = await Handler.GetByIdAsync(request);

                if (response.IsSuccess && response.Data is not null)
                {
                    InputModel = new()
                    {
                        Id = response.Data.Id,
                        Title = response.Data.Title,
                        Description = response.Data.Description,
                    };
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
        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var result = await Handler.UpdateAsync(InputModel);
                if (result.IsSuccess)
                {
                    Snackbar.Add("Categoria atualizada com sucesso!", Severity.Success);
                    NavigationManager.NavigateTo("/categorias");
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
    }
}
