using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Orders
{
    public partial class DetailsPage : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string Number { get; set; } = string.Empty;
        #endregion
        #region Properties
        public Order Order { get; set; } = null!;
        #endregion

        #region Services
        [Inject]
        public IOrderHandler Handler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            var request = new GetOrderByNumberRequest()
            {
                Number = Number,
            };
            var result = await Handler.GetByNumberAsync(request);
            if (!result.IsSuccess)
                Snackbar.Add(result.Message!, Severity.Error);
            else
                Order = result.Data!;
        }
        #endregion

        #region Methods
        public void RefreshState(Order order)
        {
            Order = order;
            StateHasChanged();
        }
        #endregion
    }
}
