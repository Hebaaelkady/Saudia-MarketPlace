using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Kader.Middlewares.Cookies
{
    public class Cookies : ICookies
    {
        private readonly HttpContext _context;
        private readonly IHttpContextAccessor accessor;
        public Cookies(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
            _context = accessor.HttpContext;
        }

        public async Task<bool> Add(string Name, string Value, double? expireTime = 440)
        {

            if (_context != null)
            {
                //option = new CookieOptions { SameSite = SameSiteMode.Strict };
                //_context.Response.Cookies.Append(Name, Value, new CookieOptions { Expires=DateTime.Now.AddDays(2) });
                var option = new CookieOptions
                {
                    //SameSite = SameSiteMode.Strict,
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    Expires = DateTime.Now.AddDays(expireTime ?? 3) // Set the expiration time
                };

                _context.Response.Cookies.Append(Name, Value, option);
                if (await Read(Name) != null)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public async Task<string> Read(string Name)
        {
            string result = null;
            if (Name == null) return null;
            if (_context != null)
                result = _context.Request.Cookies[Name];
            return result;
        }

        public async Task<T> Read<T>(string Name)
        {
            try
            {
                string result = null;
                if (Name == null) return default;
                if (_context != null)
                    result = _context.Request.Cookies[Name];

                var map = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(result);

                return map;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<bool> Remove(string Name)
        {
            if (!string.IsNullOrEmpty(Name))
                return false;

            _context.Response.Cookies.Append(Name, "", new CookieOptions { Expires = DateTime.UtcNow });

            if (await Read(Name) == null)
                return true;
            else
                return false;
        }
    }
}
