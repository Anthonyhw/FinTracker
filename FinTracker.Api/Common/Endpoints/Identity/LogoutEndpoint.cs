using FinTracker.Api.Common.Api;
using FinTracker.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FinTracker.Api.Common.Endpoints.Identity
{
    public class LogoutEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/logout", HandleAsync);

        private static async Task<IResult> HandleAsync(SignInManager<User> signinManager)
        {
            await signinManager.SignOutAsync();
            return Results.Ok();
        }
    }
}
