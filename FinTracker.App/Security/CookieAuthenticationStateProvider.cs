﻿using FinTracker.Core.Models.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Linq;

namespace FinTracker.App.Security
{
    public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
    {
        private bool _isAuthenticated = false;
        private readonly HttpClient _httpClient = clientFactory.CreateClient(Configuration.HttpClientName);


        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _isAuthenticated;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _isAuthenticated = false;

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            var userInfo = await GetUser();
            if (userInfo is null)
                return new AuthenticationState(user);

            var claims = await GetClaims(userInfo);

            var id = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
            user = new ClaimsPrincipal(id);

            _isAuthenticated = true;
            return new AuthenticationState(user);
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<User?> GetUser()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<User?>("v1/identity/manage/info");
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(
                user.Claims.Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email)
                .Select(x => new Claim(x.Key, x.Value)));

            RoleClaim[]? roles;
            try
            {
                roles = await _httpClient.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
            }
            catch
            {
                return claims;
                throw;
            }
            claims.AddRange(from role in roles ?? []
                            where !string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value)
                            select new Claim(role.Type ?? "", role.Value ?? "", role.ValueType, role.Issuer, role.OriginalIssuer));
            return claims;
        }
    }
}
