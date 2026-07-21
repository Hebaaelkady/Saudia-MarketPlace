using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kader.Middlewares
{
    public class CustomClaimsPrincipalFactory<TUser> : UserClaimsPrincipalFactory<TUser> where TUser : class
    {
        public CustomClaimsPrincipalFactory(
            UserManager<TUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // Add custom claims based on your requirements
            // For example:
            identity.AddClaim(new Claim("CustomClaimType", "ClaimValue"));

            return identity;
        }
    }
}
