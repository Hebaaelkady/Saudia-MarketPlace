 
using System.Security.Claims;

namespace Kader.Infrastructure.Jwt.Interfaces
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
