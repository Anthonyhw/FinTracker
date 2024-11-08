using FinTracker.Api.Common.Api;
using FinTracker.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinTracker.Api.Common.Endpoints.Identity
{
    public class GetRolesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/roles", Handle);

        private static Task<IResult> Handle(ClaimsPrincipal user)
        {
            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return Task.FromResult(Results.Unauthorized());

            var identity = (ClaimsIdentity)user.Identity;
            var roles = identity.FindAll(identity.RoleClaimType)
            .Select(x => new
            {
                x.Issuer,
                x.OriginalIssuer,
                x.Type,
                x.Value,
                x.ValueType
            });

            return Task.FromResult<IResult>(TypedResults.Json(roles));
        }
    }
}
