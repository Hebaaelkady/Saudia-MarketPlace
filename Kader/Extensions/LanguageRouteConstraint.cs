using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Kader.Extensions
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.ContainsKey("culture"))
                return false;

            var culture = values["culture"].ToString();
            return culture == "ar" || culture == "en";
        }
    }
}
