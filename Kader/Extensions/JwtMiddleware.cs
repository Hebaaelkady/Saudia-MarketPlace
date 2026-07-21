
using Kader.DTOs.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kader.Extensions
{

    public class JwtMiddleware
    {
        private readonly IOptions<AppSettings1> _settings;
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings1> settings)
        {
            _settings = settings;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var jwToken = context.Request.Cookies["userSession"] ?? string.Empty;
                if (context.Response.Headers["Token-Expired"] != "true" && !string.IsNullOrEmpty(jwToken) && jwToken != "null")
                {
                    var login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUserDto>(HttpUtility.HtmlDecode(jwToken));
                    context.Request.Headers.Add("Authorization", "Bearer " + login.AccessToken);

                    //var login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginDto>(jwToken);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_settings.Value.signingKey);
                    var tokenValid = tokenHandler.ValidateToken(login.JwToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    context.User = tokenValid;
                    context.Items["User"] = tokenValid;
                }
                var jwToken1 = context.Request.Cookies["user"] ?? string.Empty;
                if (context.Response.Headers["Token-Expired"] != "true" && !string.IsNullOrEmpty(jwToken1) && jwToken1 != "null")
                {
                    var login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginUserDto>(HttpUtility.HtmlDecode(jwToken1));
                    context.Request.Headers.Add("Authorization", "Bearer " + login.AccessToken);

                    //var login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginDto>(jwToken1);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_settings.Value.signingKey);
                    var tokenValid = tokenHandler.ValidateToken(login.JwToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    context.User = tokenValid;
                    context.Items["User"] = tokenValid;
                }

            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }


            await _next(context);
        }
    }
}