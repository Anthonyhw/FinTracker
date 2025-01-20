using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Orders
{
    public partial class CheckoutPage : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string productSlug { get; set; } = string.Empty;
        [SupplyParameterFromQuery]
        public string? VoucherCode { get; set; }
        #endregion

        #region Properties
        public bool IsBusy { get; set; }
        public bool IsValid { get; set; }
        public CreateOrderRequest InputModel { get; set; } = new();

        public Product? Product { get; set; }
        public Voucher? Voucher { get; set; }
        public decimal Total { get; set; }
        #endregion

        #region Services
        [Inject]
        public IProductHandler ProductHandler { get; set; } = null!;
        [Inject]
        public IOrderHandler OrderHandler { get; set; } = null!;
        [Inject]
        public IVoucherHandler VoucherHandler { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        #endregion

        #region Methods
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var request = new GetProductBySlugRequest
                {
                    Slug = productSlug
                };
                var result = await ProductHandler.GetBySlugAsync(request);
                
                if (!result.IsSuccess)
                {
                    Snackbar.Add("Não foi possível obter o produto.", Severity.Error);
                    IsValid = false;
                    return;
                }
                
                Product = result.Data;
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                IsValid = false;
                return;
            }

            if (Product is null)
            {
                Snackbar.Add("Não foi possível obter o produto.", Severity.Error);
                IsValid = false;
                return;
            }

            if (!string.IsNullOrEmpty(VoucherCode))
            {
                try
                {
                    var request = new GetVoucherByNumberRequest()
                    {
                        Code = VoucherCode ?? ""
                    };
                    var result = await VoucherHandler.GetByNumberAsync(request);

                    if (!result.IsSuccess)
                    {
                        VoucherCode = String.Empty;
                        Snackbar.Add($"Não foi possível obter o Cupom.", Severity.Error);
                    }

                    Voucher = result.Data;

                }
                catch
                {
                    VoucherCode = String.Empty;
                    Snackbar.Add("Não foi possível obter o Cupom.", Severity.Error);
                }
            }

            IsValid = true;
            Total = Product.Price - (Voucher?.Amount ?? 0);
        }

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                var request = new CreateOrderRequest() 
                {
                    ProductId = Product!.Id,
                    VoucherId = Voucher?.Id ?? null
                };

                var result = await OrderHandler.CreateAsync(request);
                if (result.IsSuccess)
                    NavigationManager.NavigateTo($"/pedidos/{result.Data!.Code}");
                else
                    Snackbar.Add(result.Message, Severity.Error);
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
