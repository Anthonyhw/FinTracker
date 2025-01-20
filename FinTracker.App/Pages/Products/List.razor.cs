using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Products
{
    public partial class ListProductsPage : ComponentBase
    {
        #region Properties
        public bool IsBusy { get; set; } = false;
        public List<Product> Products { get; set; } = [];

        #endregion

        #region Services
        [Inject]
        public IProductHandler Handler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            try
            {
                var request = new GetAllProductsRequest();
                var result = await Handler.GetAllAsync(request);

                if (result.IsSuccess)
                    Products = result.Data ?? [];
                else
                    Snackbar.Add(result.Message ?? "", Severity.Error);
            }
            catch (Exception e)
            {
                    Snackbar.Add(e.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
