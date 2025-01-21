using FinTracker.App.Pages.Orders;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Components.Orders
{
    public partial class OrderActionComponent : ComponentBase
    {
        #region Parameters
        [CascadingParameter]
        public DetailsPage DetailsPage { get; set; } = null!;
        [Parameter, EditorRequired]
        public Order Order { get; set; } = null!;
        #endregion

        #region Services
        [Inject]
        public IDialogService DialogService { get; set; } = null!;
        [Inject]
        public IOrderHandler OrderHandler { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Methods
        public async void OnCancelButtonClickedAsync()
        {
            bool? result = await DialogService.ShowMessageBox("ATENÇÃO", "Deseja realmente cancelar o pedido?", yesText: "Sim", cancelText: "Não");

            if (result is not null & result == true)
            {
                await CancelOrderAsync();
            }
        }

        private async Task CancelOrderAsync()
        {
            var request = new CancelOrderRequest()
            {
                Id = Order.Id
            };
            var result = await OrderHandler.CancelAsync(request);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message!, Severity.Success);
                DetailsPage.RefreshState(result.Data!);
            }
            else
                        Snackbar.Add(result.Message!, Severity.Error);
        }

        public async void OnRefundButtonClickedAsync()
        {
            bool? result = await DialogService.ShowMessageBox("ATENÇÃO", "Deseja realmente solicitar o reembolso?", yesText: "Sim", cancelText: "Não");

            if (result is not null & result == true)
            {
                await RefundOrderAsync();
            }
        }

        private async Task RefundOrderAsync()
        {
            var request = new RefundOrderRequest()
            {
                Id = Order.Id
            };
            var result = await OrderHandler.RefundAsync(request);
            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message!, Severity.Success);
                DetailsPage.RefreshState(result.Data!);
            }
            else
                Snackbar.Add(result.Message!, Severity.Error);
        }

        public async void OnPayButtonClickedAsync()
        {
            await PayOrderAsync();
        }

        private async Task PayOrderAsync()
        {
            await Task.Delay(1000);
            Snackbar.Add("Necessita ser implementado.");
        }
        #endregion
    }
}
