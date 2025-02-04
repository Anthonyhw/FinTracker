using FinTracker.Api.Common.Api;
using FinTracker.Api.Models;
using FinTracker.Core.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinTracker.Api.Endpoints.Identity
{
    public class GetClaimsEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/claims", Handle);

        private static Task<IResult> Handle(ClaimsPrincipal user)
        {
            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return Task.FromResult(Results.Unauthorized());;

            var claims = user.Claims.ToList();
            var result = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                result.Add(claim.Type, claim.Value);
            }

            return Task.FromResult<IResult>(TypedResults.Json(result));
        }
    }
}
