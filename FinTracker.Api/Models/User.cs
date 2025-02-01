using Microsoft.AspNetCore.Identity;

namespace FinTracker.Api.Models
{
    public class User : IdentityUser<long>
    {
        public List<IdentityRole<long>>? Roles { get; set; }
        public DateTime? PremiumExpirationDate { get; set; }
    }
}
