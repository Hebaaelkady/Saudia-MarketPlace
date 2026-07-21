using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
////using Data.DataAccessLayer.Entities;

namespace Kader.Infrastructure.Jwt.Interfaces
{
    public interface IJwtFactory
    {
        Task<Token> GenerateEncodedToken(string id, string userName, string fireBaseToken);
        Task<Token> Authorize(IdentityUser KaderUsers, HttpContext context, params Claim[] claims);
       // Task<Token> Authorize1(KaderTeachers KaderTeachers, HttpContext context, params Claim[] claims);

    }
}
