using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Kader.Infrastructure.Jwt.authentication;
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.Infrastructure.Jwt;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

namespace Kader.Infrastructure.Jwt.authentication
{
    internal sealed class JwtFactory : IJwtFactory
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtIssuerOptions _jwtOptions;

        internal JwtFactory(IJwtTokenHandler jwtTokenHandler, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<Token> GenerateEncodedToken(string id, string userName, string fireBaseToken)
        {
            var identity = GenerateClaimsIdentity(id, userName, fireBaseToken);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.UserName),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id),
                 identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.FirebaseToken)
             };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            var access = _jwtTokenHandler.WriteToken(jwt);

            return new Token(access, (int)_jwtOptions.ValidFor.TotalSeconds);
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName, string firebaseToken)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "UserName"), new[]
            {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Constants.Strings.JwtClaimIdentifiers.UserName, userName),
                new Claim(Constants.Strings.JwtClaimIdentifiers.FirebaseToken, firebaseToken)
            });
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }


        public async Task<Token> Authorize(IdentityUser user, HttpContext context, params Claim[] claims)
        {
            List<Claim> listClaims = new List<Claim>();
            foreach (var item in claims)
                listClaims.Add(item);

            listClaims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
            });

            //var loginClaims = new ClaimsIdentity(new GenericIdentity(user.UserName, "UserName"), listClaims.ToArray());

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            var access = _jwtTokenHandler.WriteToken(jwt);

            //loginClaims.AddClaim(new Claim("JwToken", access));

            //ClaimsPrincipal c_user = new ClaimsPrincipal();
            //c_user.AddIdentity(loginClaims);
            return new Token(access, (int)_jwtOptions.ValidFor.TotalSeconds);
        }
    }
}
