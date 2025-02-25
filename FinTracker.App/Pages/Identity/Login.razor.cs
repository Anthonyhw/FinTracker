﻿using FinTracker.App.Security;
using FinTracker.Core.Handlers;
using FinTracker.Core.Requests.Account;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinTracker.App.Pages.Identity
{
    public partial class LoginPage : ComponentBase 
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
        public LoginRequest Request { get; set; } = new LoginRequest();

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
                var result = await Handler.LoginAsync(Request);

                if (result.IsSuccess)
                {
                    await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                    NavigationManager.NavigateTo("/");
                }
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
