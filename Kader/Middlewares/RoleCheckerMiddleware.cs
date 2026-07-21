using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kader.Middlewares
{
    /// <summary>

    /// </summary>
    public class RoleCheckerMiddleware : IMiddleware
    {
        private readonly ILogger<RoleCheckerMiddleware> _logger;

        public RoleCheckerMiddleware(ILogger<RoleCheckerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Log the requested path
            //_logger.LogInformation($"Requested Path: {context.Request.Path}");
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var stringPaths = context.User.FindFirst("Paths")?.Value ?? "[]";
            List<string> pathsList = (JsonConvert.DeserializeObject<string[]>(stringPaths)).ToList();

            // The path to check within the list
            
            
            
            var pattern3 = "/Account/ReturnOrderDetails".ToLower();

            
            var targetPath2 = "Account/OrderDetails";
            var pattern2 = "/Account/OrderDetails/".ToLower();
            var path = context.Request.Path.ToString().ToLower();

            var targetPath1 = "Product/EditShippingPrice";
            // Check if the pathsList contains the targetPath
            if (pathsList.Any(p => p.ToLower().Contains(targetPath1.ToLower())))
            {
                var pattern1 = "/backend/Product/EditShippingPrice/".ToLower();
                  if (path.Contains(pattern1))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern1) + pattern1.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath1 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                // If found, add "/3049" to the targetPath and add it to the list
            }

            var targetPath12 = "Account/EditUser";
            // Check if the pathsList contains the targetPath
            if (pathsList.Any(p => p.ToLower().Contains(targetPath12.ToLower())))
            {
                var pattern12 = "/backend/Account/EditUser/".ToLower();
                if (path.Contains(pattern12))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern12) + pattern12.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath12 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                // If found, add "/3049" to the targetPath and add it to the list
            }
            var targetPath9 = "product/product_gomla_details";
            // Check if the pathsList contains the targetPath
            if (pathsList.Any(p => p.ToLower().Contains(targetPath9.ToLower())))
            {
                var pattern9 = "/backend/product/product_gomla_details/".ToLower();
                if (path.Contains(pattern9))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern9) + pattern9.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath9 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                // If found, add "/3049" to the targetPath and add it to the list
            }
            var targetPath91 = "Orders/Receipt";
            // Check if the pathsList contains the targetPath
            if (pathsList.Any(p => p.ToLower().Contains(targetPath91.ToLower())))
            {
                var pattern91 = "/backend/Orders/Receipt/".ToLower();
                if (path.Contains(pattern91))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern91) + pattern91.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath91 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                // If found, add "/3049" to the targetPath and add it to the list
            }
            var targetPath11 = "product/product_qta3a_details";
            // Check if the pathsList contains the targetPath
            if (pathsList.Any(p => p.ToLower().Contains(targetPath11.ToLower())))
            {
                var pattern11 = "/backend/product/product_qta3a_details/".ToLower();
                if (path.Contains(pattern11))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern11) + pattern11.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath11 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                // If found, add "/3049" to the targetPath and add it to the list
            }
            var targetPath4 = "ReturnOrders/ReturnOrdersDetails";
            if (pathsList.Any(p => p.ToLower().Contains(targetPath4.ToLower())))
            {
                var pattern4 = "/backend/ReturnOrders/ReturnOrdersDetails/".ToLower();
                if (path.Contains(pattern4))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern4) + pattern4.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath4 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }
                

            }


            var targetPath3 = "ReturnOrders/RefundMoney";
            if (pathsList.Any(p => p.ToLower().Contains(targetPath3.ToLower())))
            {
                var pattern5 = "/backend/ReturnOrders/RefundMoney/".ToLower();
                if (path.Contains(pattern5))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern5) + pattern5.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath3 + "/" + idPart;
                    pathsList.Add(updatedPath);
                }


            }
            var targetPath = "Orders/OrderDetails";
            if (pathsList.Any(p => p.ToLower().Contains(targetPath.ToLower())))
            {

                var pattern = "/backend/Orders/OrderDetails/".ToLower(); 
            
                if (path.Contains(pattern))
                {
                    var idString = path.Substring(path.LastIndexOf(pattern) + pattern.Length);
                    // Split the string at the '?' to separate the ID from any query parameters
                    var idPart = idString.Split('?')[0];
                    var updatedPath = targetPath + "/" + idPart;
                    pathsList.Add(updatedPath);
                }

        

            }
            //if (pathsList.Any(p => p.ToLower().Contains(pattern2.ToLower())))
            //{

            //    var pattern = "/backend/Orders/OrderDetails/".ToLower();
            //    var pattern1 = "/backend/Product/EditShippingPrice/".ToLower();
            //    if (path.Contains(pattern4))
            //    {
            //        var idString = path.Substring(path.LastIndexOf(pattern4) + pattern4.Length);
            //        // Split the string at the '?' to separate the ID from any query parameters
            //        var idPart = idString.Split('?')[0];
            //        var updatedPath = targetPath4 + "/" + idPart;
            //        pathsList.Add(updatedPath);
            //    }
            //    if (path.Contains(pattern))
            //    {
            //        var idString = path.Substring(path.LastIndexOf(pattern) + pattern.Length);
            //        // Split the string at the '?' to separate the ID from any query parameters
            //        var idPart = idString.Split('?')[0];
            //        var updatedPath = targetPath + "/" + idPart;
            //        pathsList.Add(updatedPath);
            //    }

            //    if (path.Contains(pattern1))
            //    {
            //        var idString = path.Substring(path.LastIndexOf(pattern1) + pattern1.Length);
            //        // Split the string at the '?' to separate the ID from any query parameters
            //        var idPart = idString.Split('?')[0];
            //        var updatedPath = targetPath1 + "/" + idPart;
            //        pathsList.Add(updatedPath);
            //    }
            //    if (path.Contains(pattern1))
            //    {
            //        var idString = path.Substring(path.LastIndexOf(pattern1) + pattern1.Length);
            //        // Split the string at the '?' to separate the ID from any query parameters
            //        var idPart = idString.Split('?')[0];
            //        var updatedPath = targetPath1 + "/" + idPart;
            //        pathsList.Add(updatedPath);
            //    }
            //    // If found, add "/3049" to the targetPath and add it to the list

            //}

            // Serialize the list back to JSON if needed
            stringPaths = JsonConvert.SerializeObject(pathsList);
            if (context.Request.Path.StartsWithSegments("/Backend", StringComparison.OrdinalIgnoreCase))
            {
                // The request is for the Backend area
                if (pathsList.Count > 0 || context.Request.Path.ToString().ToLower().EndsWith("Login".ToLower()))

                {
                    if (!pathsList.Any(rd => context.Request.Path.ToString().ToLower().EndsWith(rd.ToLower()) || context.Request.Path.ToString().ToLower().EndsWith(rd.ToLower() + "/Backend/Homes/Indexs".ToLower())) && !context.Request.Path.ToString().ToLower().EndsWith("UnAuthorized".ToLower())
                                   && !context.Request.Path.ToString().ToLower().EndsWith("Login".ToLower()) && !context.Request.Path.ToString().ToLower().Equals("/") && !context.Request.Path.ToString().ToLower().Equals("/Backend/Homes/Indexs".ToLower()))
                    {
                        context.Response.Redirect("/Backend/Homes/Indexs");
                    }
                    else
                    {
                        // Check if the path contains the pattern
                    }
                }
                else
                {
                    context.Response.Redirect("/Backend/Account/Login");
                }
            }
            else

            {
                stringPaths = JsonConvert.SerializeObject(pathsList);

                // The request is for the Backend area
                if (pathsList.Count > 0)

                {
                    //                 bool containsSpecifiedPath = pathsList.Any(path =>
                    //    path.ToLower().EndsWith("/Account/AccountCustomer".ToLower()) ||
                    //    path.ToLower().EndsWith("/Account/AddAddress".ToLower()) ||
                    //    path.ToLower().EndsWith("/Account/AllAddress".ToLower()) ||
                    //    path.ToLower().EndsWith("/Account/AllOrders".ToLower()) ||
                    //    path.ToLower().EndsWith("/Account/OrderDetails".ToLower()) ||
                    //    path.ToLower().EndsWith("/Cart/checkout".ToLower())
                    //);

                    //if (!containsSpecifiedPath)
                    //{
                    //    context.Response.Redirect("/Cart/checkout");
                    //}
                    //else
                    //{
                    //    context.Response.Redirect("/Home/Index");
                    //}
                }
                else if (context.Request.Path.ToString() == "/Cart/PaymentsRedirect")
                {
                    await next(context);
                }
                else if (/*pathsList.Count > 0 ||*/ ((context.User.Identity.IsAuthenticated == false) && context.Request.Path.ToString().ToLower().EndsWith("/Account/ReturnOrders".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/ReturnOrderDetails".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/AccountCustomer".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/ReturnOrders?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/AddAddress?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/AllAddress?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("Account/AllOrders?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/AddAddress".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/AllAddress".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("Account/AllOrders".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("Account/ReturnOrdersAll".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/OrderDetails".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Cart/checkout?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Account/ReturnOrdersAll?class=nav-link".ToLower()) || context.Request.Path.ToString().ToLower().EndsWith("/Cart/checkout".ToLower()) || context.Request.Path.ToString().ToLower().Contains(pattern3) || context.Request.Path.ToString().ToLower().Contains(pattern2)) || context.Request.Path.ToString().ToLower().Contains(pattern2) || context.Request.Path.ToString().ToLower().Contains(pattern3))
                {

                    context.Response.Redirect("/Account/Login");
                }
            }
            await next(context);
        }
    }
}
