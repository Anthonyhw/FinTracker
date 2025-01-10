using FinTracker.App.Security;
using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Identity
{
    public partial class RegisterPage: ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IAccountHandler Handler { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;


        public bool isBusy { get; set; } = false;
        public RegisterRequest Request { get; set; } = new RegisterRequest();

        protected async override Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
            {
                NavigationManager.NavigateTo("/");
            }
        }


        public async Task OnValidSubmitAsync()
        {
            isBusy = true;
            try
            {
                var result = await Handler.RegisterAsync(Request);

                if (result.IsSuccess)
                    NavigationManager.NavigateTo("/login");
                else
                    Snackbar.Add(result.Message ?? "Erro ao processar operação.", Severity.Error);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                isBusy = false;
            }
        }
    }
}
