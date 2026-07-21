
using DocumentFormat.OpenXml.Vml;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Middlewares.Cookies;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.Order;
using Kader.Services.Implementations.OrderItems;
using Kader.Services.Implementations.ProductImg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Kader.Services.Implementations.ShippingPrice;
using Kader.Services.Implementations.Address;
using Kader.DTOs.Address;
using Kader.DTOs.Orderss;
using DocumentFormat.OpenXml.InkML;
using Kader.Services.Implementations.OrderStatus;
using Kader.Infrastructure.Jwt;
using Kader.Services.Implementations.User;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;
using Kader.DTOs.Payment;
using Kader.Services.Implementations.Payment;
using Kader.DTOs.OrderItems;
using DocumentFormat.OpenXml.Office2010.Excel;
using Grpc.Core;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Kader.Middlewares.Hub;
namespace Kader.Controllers
{


    public class CartController : Controller
    {
        private readonly ILogger<ApplicationLogs> _logger;
        private IPaymentService _PaymentService;
        private readonly IHubContext<OrderHub> _hubContext;
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor; private readonly ICookies _cookies;
        private ICatogryService _Catogry; private IShippingPriceService _ShippingPrice;
        private IProductImgService _ProductImg;
        private IOrderssService _OrderService;
        private IOrderStatusService _OrderStatusService;
        private IOrderItemsService _OrderItemsService;
        private IProductService _Product; private IKeys _keys;
        private IAddressService _AddressService;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public CartController(ILogger<ApplicationLogs> logger,   IHubContext<OrderHub> hubContext, IAuthenticateService userService, IPaymentService PaymentService, IOrderStatusService OrderStatusService, ICatogryService CatogryService, IOrderssService OrderService, IOrderItemsService OrderItemsService, IAddressService AddressService, IShippingPriceService ShippingPriceService, IHttpContextAccessor httpContextAccessor, ICookies cookies, IProductImgService ProductImgService, IProductService ProductService)
        {
            _userService = userService; 
            _OrderStatusService = OrderStatusService; _Product = ProductService; _ShippingPrice = ShippingPriceService;
            _PaymentService = PaymentService; _hubContext = hubContext;
            _ProductImg = ProductImgService; _OrderItemsService = OrderItemsService;
            _AddressService = AddressService; _OrderService = OrderService; _OrderService = OrderService;
            _httpContextAccessor = httpContextAccessor; _cookies = cookies; _keys = new Keys();
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> SavePayment1(CombinedDto combinedDto)
        {

            var paymentEntity = new PaymentDto
            {
                Id = combinedDto.Payment.Id,
                Amount = combinedDto.Payment.Amount,
                Status = combinedDto.Payment.Status,
                Description = combinedDto.Payment.Description,
                Currency = combinedDto.Payment.Currency,
            };

            var SavePayment = await _PaymentService.SavePayment(_httpContextAccessor.HttpContext, paymentEntity);
            if (SavePayment.Result != null && SavePayment.isSuccess)
            {
                if (combinedDto.Checkout != null)
                {
                    combinedDto.Checkout.PaymentId = SavePayment.Result;
                    var saveModelResult = await _OrderService.SaveOrder(_httpContextAccessor.HttpContext, combinedDto.Checkout);
                    if (saveModelResult.isSuccess)
                    {
                        if (saveModelResult.Result != 0)
                        {
                            var saveModelResultw = await _OrderItemsService.SaveOrderItems(_httpContextAccessor.HttpContext, combinedDto.Checkout.Itemslist.ToList(), saveModelResult.Result);
                            return StatusCode(201, new { isSuccess = true, orderId = combinedDto.Payment.Id });

                        }


                    }
                }
            }
            // Save the model data

            return RedirectToAction("Failure");
            // Return HTTP status code 201 Created
        }
        [HttpPost]
        public async Task<IActionResult> CheckStockAvailability(SaveCheckoutDto checkout)
        {
            var CheckStockAvailabilityAsync = await _Product.CheckStockAvailabilityAsync(checkout.Itemslist);
            if (CheckStockAvailabilityAsync)
                return StatusCode(200, new { isSuccess = true });
            else
                return StatusCode(200, new { isSuccess = false });

        }
        [HttpPost]
        public async Task<IActionResult> SavePayment2([FromBody] CombinedDto combinedDto)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid: {@ModelState}", ModelState);
                return Json(new { isSuccess = false, message = "Invalid data." });
            }
            _logger.LogError("test test test ", combinedDto.Payment.Id);
            if (combinedDto == null)
            {
                return Json(new { isSuccess = false, message = "Failed to save payment details." });
            }
            try
            {
                _logger.LogError("test Payment ID: ", combinedDto.Payment.Id);


                var paymentResult = await _PaymentService.SavePayment(_httpContextAccessor.HttpContext, combinedDto.Payment);
                if (!paymentResult.isSuccess)
                {
                    _logger.LogError("Payment save failed. Payment ID: {Id}", combinedDto.Payment.Id);
                  //  return Json(new { isSuccess = false, message = "Failed to save payment details." });
                }

                PaymentDto payment = new PaymentDto();
                if (paymentResult.isSuccess)
                {
                    combinedDto.Checkout.PaymentId = paymentResult.Result;

                    // Step 2: Save Order with retries
                    var orderSaved = await SaveOrderWithRetry(combinedDto.Checkout);

                    if (!orderSaved)
                    {

                        payment.StatusOrder = 7;
                         
                        payment.Id = combinedDto.Payment.Id;
                        _PaymentService.UpdatePayment(payment );
                        _logger.LogError("SaveOrderWithRetry. Payment ID: {Id}", combinedDto.Payment.Id);

                        return StatusCode(202, "Order save deferred for background processing.");
                    }

                    // Step 3: Assign OrderId from SaveOrderWithRetry
                    var orderId = combinedDto.Checkout.Idorders;

                    // Step 4: Save Order Items with retries
                    var itemsSaved = await SaveOrderItemsWithRetry(combinedDto.Checkout.Itemslist.ToList(), orderId);
                 
                    if (!itemsSaved)
                    {
                       
                        payment.StatusOrderItems =8;

                        payment.Id = combinedDto.Payment.Id;
                         _PaymentService.UpdatePayment(payment);
                        _logger.LogError("SaveOrderItemsWithRetry. Payment ID: {Id}", combinedDto.Payment.Id);

                        return StatusCode(202, "Items save deferred for background processing.");
                    }
                     
                    payment.StatusOrder = 9;
                    payment.StatusOrderItems = 9;
                    payment.Id = combinedDto.Payment.Id;
                     _PaymentService.UpdatePayment(payment);

                    // Return success with the created Order ID
                    return Json(new { isSuccess = true });
                    //return StatusCode(201, new { isSuccess = true, orderId });
                }}
               
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "SavePayment failed.");
                return Json(new { isSuccess = false, message = "Stock unavailable." });
            }
            _logger.LogError("test Payment ID1: ", combinedDto.Payment.Id);
            return Json(new { isSuccess = false, message = "Stock unavailable." });
        }
     
        private async Task<bool> SaveOrderWithRetry(SaveCheckoutDto checkout)
        {
            return await RetryAsync(
                async () =>
                {
                    checkout.StatusId = 11;
                    var orderResult = await _OrderService.SaveOrder(_httpContextAccessor.HttpContext, checkout);
                    if (orderResult.isSuccess)
                    {
                        checkout.Idorders = orderResult.Result; // Save the generated Order ID in the DTO
                        return true;
                    }
                    return false;
                },
                maxRetries: 3,
                delayInMilliseconds: 2000 // Retry every 2 seconds
            );
        }
        private async Task<bool> SaveOrderItemsWithRetry(List<Itemslist> items, int orderId)
        {
            return await RetryAsync(
                async () =>
                {
                    var itemsResult = await _OrderItemsService.SaveOrderItems(_httpContextAccessor.HttpContext, items, orderId);
                    return itemsResult.isSuccess; // Return success if items are saved
                },
                maxRetries: 3,
                delayInMilliseconds: 2000 // Retry every 2 seconds
            );
        }
        private async Task<bool> RetryAsync(Func<Task<bool>> action, int maxRetries, int delayInMilliseconds)
        {
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    if (await action())
                    {
                        return true; // Success
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Retry attempt {RetryCount} failed.", retryCount + 1);
                }

                retryCount++;
                await Task.Delay(delayInMilliseconds); // Wait before retrying
            }

            return false; // All retries failed
        }


        [HttpGet]
public async Task<IActionResult> PaymentsRedirect(string id, string status, string message, string payment1, string checkout)
{
    PaymentDto payment = new PaymentDto
    {
        Status = status,
        Message = message,
        Id = id
    };

    try
    {
        // Update payment status
        var updatePaymentResult = _PaymentService.UpdatePayment(payment);
        if (!updatePaymentResult.isSuccess)
        {
            // Log error and show a meaningful message
            _logger.LogError("Failed to update payment. Payment ID: {Id}, Status: {Status}, Message: {Message}", id, status, message);
                    ViewBag.Error = "Failed to process your payment. Please contact support.";
            return RedirectToAction("Failure");
        }

        // Handle paid status
        if (status == "paid")
        {
            var orderItemsResult = await _OrderItemsService.GetOrderItems(id);
            if (!orderItemsResult.isSuccess)
            {
                _logger.LogError("Failed to retrieve order items for Payment ID: {Id}.", id);
                        ViewBag.Error = "Unable to retrieve your order items. Please try again.";
                return RedirectToAction("Failure");
            }

            // Decrease stock quantity
            var isStockDecreased = await _Product.DecreaseQuantity(orderItemsResult.Result);
            if (!isStockDecreased.isSuccess)
            {
                _logger.LogError("Failed to decrease stock quantity for order items. Payment ID: {Id}.", id);
                        ViewBag.Error = "Some items in your order are no longer available. Please contact support.";
                return RedirectToAction("Failure");
            }

            // Update order status
            var updateStatusResult = await _OrderService.updateStatusWithPayID(1, id);
            if (!updateStatusResult.isSuccess)
            {
                _logger.LogError("Failed to update order status. Payment ID: {Id}.", id);
                        ViewBag.Error = "Failed to finalize your order. Please contact support.";
                return RedirectToAction("Failure");
            }

            // Clear the cart cookie
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("cartData");
                    await _hubContext.Clients.All.SendAsync("ReceiveNewOrderNotification", id);

                    // Redirect to order details
                    return RedirectToAction("OrderDetails", "Account", new { id = id });
        }
    }
    catch (Exception ex)
    {
        // Catch unexpected exceptions and log them
        _logger.LogError(ex, "An unexpected error occurred while processing Payment ID: {Id}.", id);
                ViewBag.Error = "An unexpected error occurred. Please try again later.";
    }

    // Redirect to the failure page if something goes wrong
    return RedirectToAction("Failure");
}
            [HttpGet("Cart/GetPublishableApiKey")]
        public IActionResult GetPublishableApiKey()
        {
           

            //var apiKey = "pk_test_mqKjAYvAJn6xqRaHvRpHVQ5JcRF9i5LXk5qEH6Ys";
            var apiKey = "pk_live_wLVEej7pbtpKP76wpsA59eTfjjZCR8Vrpx3VFtkG";
            if (string.IsNullOrEmpty(apiKey))
            {
                return NotFound("API Key not found");
            }

            return Ok(new { publishable_api_key = apiKey });
        }
        public IActionResult LoadCartPartialQt3(ApiEditProductDto model)
        {
            var model1 = new ProductView_front();
            model1.Productsitem = model;
            return PartialView("../Cart/_CartQt3", model1);
        }
        public IActionResult LoadCartPartial(ApiEditProductDto model)
        {
            var model1 = new ProductView_front();
            model1.Productsitem = model;
            return PartialView("../Cart/_Cart", model1);
        }
        public async Task<IActionResult> LoadCart(int productId, int ApiProdID)
        {
            var viewModel = new Kader.DTOs.Product.ProductView_front();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                if (ApiProdID != 0)
                {
                    var GetProduct = await _Product.GetProductByIDApi(token, ApiProdID);
                    if (GetProduct.isSuccess)
                    {


                        var product = _Product.GetSingleProducts(_httpContextAccessor.HttpContext, productId, null).Result;
                        if (product.isSuccess)
                        {
                            if (product.Result != null)
                            {
                                viewModel.Productsitem = GetProduct.Result; ;

                                viewModel.Productsitem.title = GetProduct.Result.title;
                                viewModel.Productsitem.barcode = GetProduct.Result.barcode;
                                //viewModel.barcode = GetProduct.Result.barcode;
                                viewModel.Productsitem.stock = GetProduct.Result.stock;
                                viewModel.Productsitem.price = GetProduct.Result.price;
                                viewModel.Productsitem.unites1 = GetProduct.Result.unites.FirstOrDefault().unitName;
                                return PartialView("~/Views/Cart/_Cart.cshtml", viewModel);
                            }
                        }
                    }
                }

            }
            //var GetSingleProduct = _Product.GetSingleProducts(_httpContextAccessor.HttpContext,productId,null).Result;
            //if(GetSingleProduct.isSuccess)
            //{
            //    var productView = new Kader.DTOs.Product.ProductView_front
            //    {
            //        Productsitem = GetSingleProduct.Result
            //    };

            //}
            return NotFound();
        }
        public IActionResult Failure()
        {
            return View();
        }

        public IActionResult _Cart()
        {
            return View();
        }
        public IActionResult _CartQt3()
        {
            return View();
        }
        public async Task<ActionResult> Checkout()
        {
            // Fetch cart data
            var cartData = GetCartDataFromLocalStorage();
            if (cartData == null || !cartData.Any())
            {
                return View();
            }

            // Fetch active address
            var activeAddressResponse = await _AddressService.GetActiveAddress(_httpContextAccessor.HttpContext);
            var activeAddress = activeAddressResponse.isSuccess ? activeAddressResponse.Result : null;

            // Extract product IDs from cart
            var productIds = cartData.Select(item => item.ProductId).Where(id => id != 0).ToList();
            if (!productIds.Any())
            {
                return View();
            }

            // Fetch product details
            var productsResponse = await _Product.GetProductsByprodID<Itemslist>(productIds);
            if (!productsResponse.isSuccess || productsResponse.Result == null)
            {
                ViewBag.ErrorMessage = "Unable to fetch product details.";
                return View();
            }

            foreach (var product in productsResponse.Result)
            {
                var matchingCartItem = cartData.FirstOrDefault(c => c.ProductId == product.ProductId);
                if (matchingCartItem != null)
                {

                    if(product.QuantityAvailable < 1 || (matchingCartItem.typeGomlaOrQt3 == 1 && product.MinQuantityToShipJomla == 0) || (matchingCartItem.typeGomlaOrQt3 == 2 && product.MaxQuantityToShipQta3a == 0) || (matchingCartItem.typeGomlaOrQt3 == 1 && product.MinQuantityToShipJomla > (double)product.QuantityAvailable) || (matchingCartItem.typeGomlaOrQt3 == 2 && product.MaxQuantityToShipQta3a > (double)product.QuantityAvailable))

                    {
                        product.Quantity_user = 0;
                    }
                    else
                    {
                        product.Quantity_user = matchingCartItem.Quantity; // Update Quantity_user based on cart data

                    }
                    product.typeGomlaOrQt3 = matchingCartItem.typeGomlaOrQt3;
                    if (product.AfterDiscount != null)
                    {
                        double price = product.BeforeDiscount;
                        double afterDiscount = /*price - (price **/ (double)product.AfterDiscount.Value /*/ 100)*/;
                        product.PriceItem = Math.Round(afterDiscount, 2); // Set PriceItem after rounding
                    }
                    else
                    {
                        product.PriceItem = product.BeforeDiscount; // If no discount, PriceItem equals the original price
                    }
                }
            }


            // Prepare checkout view model
            var viewModel = new Checkout_frontDto
            {
                Itemslist = productsResponse.Result,
                AddressDto = activeAddress
            };

     

            return View(viewModel);
        }

        //[HttpPost]
        //public async Task<ActionResult> checkout(Checkout_frontDto model)
        //{

        //    if (model == null)
        //    {
        //        return View(model);
        //    }
        //    else
        //    {
        //        ReturnDto<int> SaveOrder;
        //        //var DecreaseQuantity = _Product.DecreaseQuantity(_httpContextAccessor.HttpContext, model.Itemslist).Result;

        //        double totalwithoutshipp = 0;
        //        double totalShippingPrice = 0;



        //        foreach (var r in model.Itemslist)
        //        {
        //            totalwithoutshipp = totalwithoutshipp + (r.Quantity * r.PriceItem).Value;
        //            totalShippingPrice = totalShippingPrice + r.ShippingPrice.Value;
        //        }
        //        model.TotalTransportShippPrice = totalShippingPrice;
        //        model.TotalPrice = totalwithoutshipp;
        //        //model.Itemslist = DecreaseQuantity.Result;
        //        model.StatusId = 1;
        //        SaveOrder = await _OrderService.SaveOrder(_httpContextAccessor.HttpContext, model);
        //        if (SaveOrder.isSuccess)
        //        {
        //            var SaveOrder1 = await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, SaveOrder.Result, model.StatusId.Value);
        //            if (SaveOrder1.isSuccess)
        //            {
        //            }
        //            ReturnDto<bool> SaveOrderItems;
        //            SaveOrderItems = _OrderItemsService.SaveOrderItems(_httpContextAccessor.HttpContext, model.Itemslist, SaveOrder.Result).Result;
        //            if (SaveOrderItems.isSuccess)
        //            {
        //                var token = await _userService.GetTokenAsync();
        //                if (token != null)
        //                {
        //                    var SaveOrder2 = await _OrderService.SaveOrderApi(token, model);
        //                }
        //                TempData["SuccessMessage"] = "تم الحفظ بنجاح :)!";
        //                ModelState.Clear();
        //                _httpContextAccessor.HttpContext.Response.Cookies.Delete("cartData");
        //                return RedirectToAction("OrderDetails", "Account", new { id = SaveOrder.Result });
        //            }
        //        }

        //        else
        //        {
        //            TempData["SuccessMessage"] = "حدث خطا";
        //        }
        //    }
        //    return View(model);
        //}
        public async Task<ActionResult> ViewCart()
        {
            // 1. Get Cart Data:
            var cartData = GetCartDataFromLocalStorage();
            if (cartData == null || !cartData.Any())
            {
                return View(); // You might have an "EmptyCart" view
            }

            // 2. Get Product IDs from Cart:
            var productIds = cartData.Select(item => item.ProductId).Where(id => id != 0).ToList();
            if (!productIds.Any())
            {
                return View(); // Handle case where no valid product IDs are found
            }

            // 3. Fetch Products:
            var productsResponse = await _Product.GetProductsByprodID<ApiEditProductDto>(productIds);
            if (!productsResponse.isSuccess || productsResponse.Result == null)
            {
                ViewBag.ErrorMessage = "Unable to fetch product details.";
                return View();
            }
            var products = productsResponse.Result;
            foreach (var product in products)
            {
                var matchingCartItem = cartData.FirstOrDefault(c => c.ProductId == product.ProductId);
                if (matchingCartItem != null)
                {
                    product.Quantity_user = matchingCartItem.Quantity; // Update Quantity_user based on cart data
                    product.typeGomlaOrQt3 = matchingCartItem.typeGomlaOrQt3;
                   //product.Unit_Name = matchingCartItem.unit;
                }
            }


            return View(products);
        }

        public IActionResult AddToCart(Itemslist model)
        {
            var cartData = GetCartDataFromLocalStorage();
            if (model.Action == "delete") // Check for delete action
            {
                // Remove the product from the cart 
                cartData.RemoveAll(p => p.ProductId == model.ProductId);
            }
            else // Default is "add" or other actions
            {
                var productItem = cartData.FirstOrDefault(p => p.ProductId == model.ProductId);
                if (productItem == null)
                {
                    cartData.Add(model);
                }
                else
                {
                    if (model.Action == "add" || model.Action == "subtract" || model.Action == "same")
                    {
                        productItem.Quantity = model.Quantity;
                    }

                    else
                        productItem.Quantity += model.Quantity;
                }
            }
            SaveCartDataToLocalStorage(cartData);
            return Json(new { success = true });
        }
        public IActionResult AddToCartQt3(Itemslist model)
        {
            var cartData = GetCartDataFromLocalStorageQt3();
            if (model.Action == "delete") // Check for delete action
            {
                // Remove the product from the cart 
                cartData.RemoveAll(p => p.ProductId == model.ProductId);
            }
            else // Default is "add" or other actions
            {
                var productItem = cartData.FirstOrDefault(p => p.ProductId == model.ProductId);
                if (productItem == null)
                {
                    cartData.Add(model);
                }
                else
                {
                    if (model.Action == "add" || model.Action == "subtract" || model.Action == "same")
                    {
                        productItem.Quantity = model.Quantity;
                    }

                    else
                        productItem.Quantity += model.Quantity;
                }
            }
            // 3. Update Local Storage with the modified cart data
            SaveCartDataToLocalStorageQt3(cartData);
            //   return RedirectToAction("ViewCart");
            return Json(new { success = true });
        }

        // Helper methods to get and save cart data from Local Storage
        private List<Itemslist> GetCartDataFromLocalStorage()
        {
            var dataString = HttpContext.Request.Cookies["cartData"];
            if (dataString != null)
            {
                return JsonConvert.DeserializeObject<List<Itemslist>>(dataString);
            }
            return new List<Itemslist>();
        }
        private List<Itemslist> GetCartDataFromLocalStorageQt3()
        {
            var dataString = HttpContext.Request.Cookies["cartDataQt3"];
            if (dataString != null)
            {
                return JsonConvert.DeserializeObject<List<Itemslist>>(dataString);
            }
            return new List<Itemslist>();
        }
        private void SaveCartDataToLocalStorage(List<Itemslist> cartData)
        {
            var dataString = JsonConvert.SerializeObject(cartData);
            //var cookieOptions = new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true  // Requires HTTPS
            //};

            //_cookies.Append("cartData", dataString, cookieOptions);

            // _cookies.Add("myCookie", "data", new CookieOptions { Expires = DateTime.Now.AddDays(7) });  // Expires in 7 days

            HttpContext.Response.Cookies.Append("cartData", dataString,
                    new CookieOptions
                    {
                        //   HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true // Requires HTTPS
                                        ,
                        Expires = DateTime.UtcNow.AddHours(3).AddDays(7)
                    });
            //var encryptedData = EncryptData(data);

            //Response.Cookies.Append("secureCookie", encryptedData, cookieOptions);
        }
        private void SaveCartDataToLocalStorageQt3(List<Itemslist> cartData)
        {
            var dataString = JsonConvert.SerializeObject(cartData);
            HttpContext.Response.Cookies.Append("cartDataQt3", dataString,
                    new CookieOptions
                    {
                        //   HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true // Requires HTTPS
                                        ,
                        Expires = DateTime.UtcNow.AddHours(3).AddDays(7)
                    });
        }


    }
}
