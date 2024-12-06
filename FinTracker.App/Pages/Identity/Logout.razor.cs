using FinTracker.App.Security;
using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Identity
{
    public partial class LogoutPage : ComponentBase
    {
        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;
        [Inject]
        public IAccountHandler Handler { get; set; } = null!;
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        protected async override Task OnInitializedAsync()
        {
            if (await AuthenticationStateProvider.CheckAuthenticatedAsync())
            {
                await Handler.LogoutAsync();
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
            }

            await base.OnInitializedAsync();
        }
    }
}
