using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Units;
using Kader.DTOs.Colors;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.ShippingPrice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kader.Services.Implementations.Units;
using Kader.Services.Implementations.Colors;
using Kader.DTOs.Catogry;
using Kader.DTOs.RoleDetail;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Services.Implementations.User;
using Kader.Services.Implementations.Order;
using Kader.DTOs.Cart;
using Kader.Services.Implementations.OrderStatus;
using Kader.DTOs.OtoApi;
using Newtonsoft.Json;
using Kader.Services.Implementations.ShippingCompany;
using Kader.Infrastructure.Jwt;
using Microsoft.Extensions.Logging;
using Kader;
using System.Net.Http;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Kader.DTOs.Orderss;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using Kader.Services.Implementations.Stores;
using DocumentFormat.OpenXml.Wordprocessing;
using Kader.Services.Implementations.UsersStores;
using Kader.Services.Implementations.UserRoles;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kader.Services.Implementations.Sms;
using Microsoft.AspNetCore.SignalR;
using DocumentFormat.OpenXml.InkML;
using Kader.Middlewares.Hub;
using Newtonsoft.Json.Linq;

[Area("Backend")]

    public class OrdersController : Controller
{
    private readonly ILogger<ApplicationLogs> _logger;
    private IAuthenticateService _userService; private ISmsService _SmsService;
    private IOrderssService _Orderss; private IUsersStoresService _UsersStores;
    IProductService _Product; IOtoApiService _OtoApiService;
    IOrderStatusService _OrderStatusService; private IStoresService _Stores;
    private IHttpContextAccessor _httpContextAccessor;
    private IColorsService _Colors; private readonly IHubContext<OrderHub> _hubContext;
    private IUnitsService _Units; private IUserRoleService _UserRole;
    private IKeys _keys; 
        public OrdersController(ILogger<ApplicationLogs> logger, IHubContext<OrderHub> hubContext, ISmsService SmsService, IUsersStoresService UsersStoresService, IUserRoleService UserRoleService, IStoresService StoresService, IAuthenticateService userService, IOtoApiService OtoApiService, IOrderStatusService OrderStatusService, IOrderssService OrderssService, IUnitsService UnitsService, IProductService ProductService, ICatogryService CatogryService, IColorsService ColorsService, IHttpContextAccessor httpContextAccessor)
        {
        _Units = UnitsService; _UserRole = UserRoleService; _userService = userService; _hubContext = hubContext;
        _OtoApiService = OtoApiService; _Stores = StoresService; _UsersStores = UsersStoresService;
        _httpContextAccessor = httpContextAccessor; _OrderStatusService = OrderStatusService;
        _Product = ProductService; _logger = logger; _SmsService = SmsService;
        _Orderss = OrderssService; _keys = new Keys();
    }
   
    public async Task<IActionResult> NewOrders()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext,1);
        


        if (GetOrderss.isSuccess)
        {
            
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> UnderCheck()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, 3);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> underShipent()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, 4);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> InnerunderShipent()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, 14);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> completedOrders()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, 15);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> NotcompletedOrders()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, 16);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> AssignedToStores()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, _keys.AssignToStore());
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> AssignedToShipping()
    {
        var GetOrderss = await _Orderss.GetAllOrderByStatus(_httpContextAccessor.HttpContext, _keys.AssignedToShipping());
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignToStore([FromForm] AssignStoreOrderDto request)
    {
        try
        {
            if (request == null)
            {
                return Json(new { success = false, message = "Invalid request: Data is null!" });
            }

            var orderStatusResult = await _OrderStatusService.GetOrderStatus(request.Idorders);
            if (!orderStatusResult.isSuccess)
            {
                return Json(new { success = false, message = "Failed to fetch order status!" });
            }

            var statusCheck = _keys.AssignToStore();
            var statusExists = orderStatusResult.Result.Any(k => k.StatusId == statusCheck);

            if (!statusExists)
            {
                request.Statusid = statusCheck;
                var updateStatusResult = await _Orderss.update(_httpContextAccessor.HttpContext, request);
                if (!updateStatusResult.isSuccess)
                {
                    return Json(new { success = false, message = "Failed to update order!" });
                }

                var saveOrderStatus = await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, request.Idorders, statusCheck, request.StoreId, null);
                if (!saveOrderStatus.isSuccess)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNewOrderNotification", request.Idorders);
                    return Json(new { success = false, message = "Failed to save order status!" });
                }

                return Json(new { success = true, message = "Order assigned to store successfully!" });
            }

            return Json(new { success = false, message = "Order already assigned to store!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, errorMessage = "An error occurred: " + ex.Message });
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssignToShipping(int Idorders, int? StoreID)
    {
        if (Idorders <= 0)
        {
            return BadRequest("❌ رقم الطلب غير صحيح.");
        }

        var statusCheck = _keys.AssignedToShipping();
        var updateStatusResult = await _Orderss.updateStatus(_httpContextAccessor.HttpContext, statusCheck, Idorders);

        if (updateStatusResult.isSuccess)
        {
            var SaveOrderStatus = await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, Idorders, statusCheck, StoreID.Value, null);

            if (SaveOrderStatus.isSuccess)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", "NewOrders", "تمت إضافة طلب جديد!");

                return Json(new { success = true, message = "تم إرسال الطلب بنجاح!" });
            }
            else
            {
                return Json(new { success = false, message = "فشل في تحديث الطلب." });
            }
        }

        return Json(new { success = false, message = "❌ حدث خطأ أثناء معالجة الطلب." });
    }
    [HttpGet]
    public async Task<JsonResult> GetUsersByStore(int storeId)
    {
        var shippingUsers = await _UserRole.GetAllShippingUsers(_httpContextAccessor.HttpContext, _keys.InsideShippingRole());

        if (shippingUsers.isSuccess && shippingUsers.Result != null)
        {
            var allUserStores = await _UsersStores.GetAllUserStores(storeId);
             
            var filteredUsers = allUserStores.Result
            .Where(us => shippingUsers.Result.Any(su => su.UserId == us.UserId))
                .Select(su => new
                {
                    Id = su.UserId,
                    FullName = $"{su.FirstName} {su.LastName}",
                    PhoneNumber = su.PhoneNumber
                    ,
                    CountryCode = su.CountryCode
                })
                .ToList();

            return Json(filteredUsers);
        }

        return Json(new List<object>()); // ✅ Always return an empty list instead of null
    }
    [HttpPost]
    public async Task<JsonResult> SaveInnerShippingOrder(string OrdersNo, int orderIds, string CustomerPhone, string InsideShippingUser, string phoneNumber, string Address, string StoreId1)
    {
        try
        {
            if (orderIds == 0 && phoneNumber == null)
            {
                return Json(new { success = false, message = "لا يوجد طلبات للتحويل." });
            }

            if (string.IsNullOrEmpty(InsideShippingUser))
            {
                return Json(new { success = false, message = "يجب تحديد مندوب الشحن الداخلي." });
            }

            DateTime ShippingDate = DateTime.UtcNow.AddHours(3);
            // Update shipment status
            var updateshipment = await _Orderss.updateshipment(orderIds, null, null, ShippingDate, null, 14, InsideShippingUser);

            if (updateshipment.isSuccess)
            {
                var SaveOrderStatus = await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, orderIds, 14, updateshipment.Result, null);
                if (SaveOrderStatus.isSuccess)
                {
                    string message = " لديك طلب رقم  " + orderIds + " من مخزن " + StoreId1 + " الي " + Address;
                    var otpResponse = await _SmsService.SendSmsAsync(phoneNumber, message);
                    if (otpResponse == null)
                    {
                        return Json(new { success = false, message = "حدث خطا اثناء ارسال رسالة لمندوب التوصيل" });
                    }
                   else if(otpResponse.code==1)
                    {
                        MsgDto c = new MsgDto();
                        c.Idorders = orderIds;
                        c.MsgToDelivery=true;
                        var updateshipment1 = await _Orderss.updateSms(_httpContextAccessor.HttpContext, c);
                    }

                    string Customermessage = "طلبكم قيد الشحن رقم استلام الطلب هو " + OrdersNo;

                    var otpResponse1 = await _SmsService.SendSmsAsync(CustomerPhone, Customermessage);
                    if (otpResponse1 == null)
                    {
                        return Json(new { success = false, message = "حدث خطا اثناء ارسال رسالة للعميل" });
                    }
                    else if (otpResponse.code == 1)
                    {
                        MsgDto c = new MsgDto();
                        if (otpResponse.code == 1)
                        {
                            c.MsgToDelivery = true;
                        }
                            c.Idorders = orderIds;
                        c.MsgCofeToUser = true;
                        var updateshipment1 = await _Orderss.updateSms(_httpContextAccessor.HttpContext, c);
                    }
                }
                return Json(new { success = true, message = "تم التحويل لمندوب الشحن الداخلي بنجاح." });
            }
            else
            {
                return Json(new { success = false, message = "فشل في تحديث الطلب." });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Json(new { success = false, message = "حدث خطأ أثناء التحويل لمندوب الشحن الداخلي." });
        }
    }

    [HttpPost]
    public async Task<JsonResult> CheckCode(int orderIds , string code)
    {
        try
        {
            if (orderIds == 0)
            {
                return Json(new { success = false, message = "لا يوجد طلبات للتحويل." });
            }
            if (code == null)
            {
                return Json(new { success = false, message = "فشل في الحفظ." });
            }
            var singleOrderResult = await _Orderss.CheckCode(_httpContextAccessor.HttpContext, code);
            if (!singleOrderResult.isSuccess)
            {
                return Json(new { success = false, message = "الكود غير مطابق." });
            }
            return Json(new { success = true, message = "الكود مطابق." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Json(new { success = false, message = "حدث خطأ  ." });
        }
    }

    [HttpPost]
    public async Task<JsonResult> saveInnerDeliveryBtn( int orderIds  , string Comment, int deliverStatus, string code)
    {
        try
        {
            if (orderIds == 0  )
            {
                return Json(new { success = false, message = "لا يوجد طلبات للتحويل." });
            }
            DateTime DeliveryDate = DateTime.UtcNow.AddHours(3);
            // Update shipment status
            var updateshipment = await _Orderss.updateshipment(orderIds, null, DeliveryDate, null, null, deliverStatus, null);

            if (updateshipment.isSuccess)
            {
                var SaveOrderStatus = await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, orderIds, deliverStatus, 0, Comment);
                if (SaveOrderStatus.isSuccess)
                {

                    return Json(new { success = true, message = "تم الحفظ بنجاح." });
                }
                 else
            {
                return Json(new { success = false, message = "فشل في الحفظ." });
            }
            }
            else
            {
                return Json(new { success = false, message = "فشل في الحفظ." });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return Json(new { success = false, message = "حدث خطأ  ." });
        }
    }
    public async Task<IActionResult> Receipt(string id)
    {


        if (id == null)
        {
            return View();
        }
        

        var singleOrderResult = await _Orderss.GetSingleOrder<OrderDetailsDto>(_httpContextAccessor.HttpContext, id);
        if (!singleOrderResult.isSuccess)
        {
            return View();
        }
         return View(singleOrderResult.Result);

    }

    public async Task<IActionResult> OrderDetails(string id)
    {
        
         
        if (id == null)
        {
            return View();
        }
        var GetStores = await _Stores.GetStores();
        if (GetStores.isSuccess)
        {
            ViewBag.Stores = GetStores.Result;
        }

        var singleOrderResult = await _Orderss.GetSingleOrder<OrderDetailsDto>(_httpContextAccessor.HttpContext, id);
        if (!singleOrderResult.isSuccess)
        {
            return View();
        }
        var roleClaims = User.Claims
                          .Where(c => c.Type == ClaimTypes.Role)
                          .Select(c => c.Value)
                          .ToList();

        // If user has any role
        if (roleClaims.Count > 0)
        {
            // Store multiple roles in a list
            var userRoles = new List<int>();

            if (roleClaims.Contains(_keys.Admin())  )
            {
                userRoles.Add(1); // Admin role
            }
            if (  roleClaims.Contains(_keys.ManagOrderRole()))
            {
                userRoles.Add(6); // Admin role
            }
            if (roleClaims.Contains(_keys.ShippingRole()))
            {
                userRoles.Add(2); // Shipping role
            }

            if (roleClaims.Contains(_keys.storeRole()))
            {
                userRoles.Add(3); // Store role
                var orderStatusResult = await _OrderStatusService.GetOrderStatus(singleOrderResult.Result.Idordersss);
                if (!orderStatusResult.isSuccess)
                {
                    return View("Error");
                }

                var statusCheck = _keys.status_check();
                var statusExists = orderStatusResult.Result.Any(k => k.StatusId == statusCheck);
                if (!statusExists)
                {
                    var updateStatusResult = await _Orderss.updateStatus(_httpContextAccessor.HttpContext, statusCheck, singleOrderResult.Result.Idordersss);
                    if (updateStatusResult.isSuccess)
                    {
                        await _OrderStatusService.SaveOrderStatus(
                            _httpContextAccessor.HttpContext,
                            singleOrderResult.Result.Idordersss,
                            statusCheck,
                            singleOrderResult.Result.StoreID,
                            null
                        );
                    }
                }
            }

            if (roleClaims.Contains(_keys.InsideShippingRole()))
            {
                userRoles.Add(4); // Default role
            }

            ViewBag.IsAdmin = userRoles; // Store multiple roles in ViewBag
        }


        if ( (singleOrderResult.Result.StatusId != 5&& singleOrderResult.Result.StatusId != 4&& singleOrderResult.Result.StatusId != 14))
        {
            // Get all users with the Shipping Role from UserRole table
            var shippingUsers = await _UserRole.GetAllShippingUsers(_httpContextAccessor.HttpContext, _keys.InsideShippingRole());

            if (shippingUsers.isSuccess && shippingUsers.Result != null)
            {
                ViewBag.StoreID = singleOrderResult.Result.StoreName;
                var shippingUsersByStore = new Dictionary<string, List<SelectListItem>>();
               if (roleClaims.Contains(_keys.ShippingRole()) )
                {
                    if (singleOrderResult.Result.StoreID != 0)
                    {
                        var storeIdFilter = singleOrderResult.Result.StoreID;
                        var allUserStores = await _UsersStores.GetAllUserStores(storeIdFilter);
                        shippingUsersByStore = allUserStores.Result
                        .Where(us => shippingUsers.Result.Any(su => su.UserId == us.UserId))
                        .GroupBy(us => us.StoreName)
                        .ToDictionary(g => g.Key, g => g.Select(us => new SelectListItem
                        {
                            Value = us.UserId.ToString(),
                            Text = $"{us.FirstName} {us.LastName} - {us.CountryCode}{us.PhoneNumber}",
                        }).ToList());
                    }
                }

                // ✅ Always assign to ViewBag to prevent errors
                ViewBag.ShippingUsersByStore = shippingUsersByStore.Any() ? shippingUsersByStore : new Dictionary<string, List<SelectListItem>>();
            }



        }
   

        if (singleOrderResult.Result.ShippingotoId != null&& !roleClaims.Contains(_keys.storeRole()))
        {
            var accessToken = await _OtoApiService.GetAccessTokenAsync();
            string ids = id.ToString();
            var deliveryOptions = await _OtoApiService.GetShipmentDetailsAsync(accessToken, singleOrderResult.Result.Idordersss.ToString());

            var orderResponseData = JsonConvert.DeserializeObject<ApiShippingorderStatus>(deliveryOptions);
            if (orderResponseData.status != null)
            {
                singleOrderResult.Result.ApiShippingorderStatus = orderResponseData;
            }
        }
            return View(singleOrderResult.Result);
        
    }
    public async Task<IActionResult> DeliveryOptions(string refreshToken)
    {
        try
        {
            // Get access token
            var accessToken = await _OtoApiService.GetAccessTokenAsync();

            // Get delivery options
            var deliveryOptions = await _OtoApiService.GetDeliveryOptionsAsync(accessToken);

            // Map to ViewModel
            var viewModel = deliveryOptions.Select(option => new DeliveryOptionDto
            {
                DeliveryOptionId = option.DeliveryOptionId,
                CompanyName = option.CompanyName,
                Fee = option.Fee,
                Description = option.Description
            }).ToList();

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Handle errors
            ViewBag.ErrorMessage = ex.Message;
            return View("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> SelectDeliveryOption(string deliveryOptionId)
    {
        // Store the selected delivery option and redirect to the next process
        TempData["SelectedDeliveryOptionId"] = deliveryOptionId;
        return RedirectToAction("NextStep");
    }

    public async Task<IActionResult> NextStep()
    {
        var selectedDeliveryOptionId = TempData["SelectedDeliveryOptionId"] as string;

        if (string.IsNullOrEmpty(selectedDeliveryOptionId))
        {
            return RedirectToAction("DeliveryOptions");
        } 

        // Proceed with the selected delivery option
        // Example: Create Pickup Location, Order, etc.

        ViewBag.Message = $"You selected Delivery Option: {selectedDeliveryOptionId}";
        return View();
    }
    [HttpPost]
    public IActionResult delivery_fee_details(CheckOTODeliveryFeeRequestDto data, string destinationCity, int weight,int Idordersss)
    {
        ViewBag.Data = data;
        return View(data);
    }

        
    public async Task<IActionResult> CheckOTODeliveryFee([FromBody] CheckOTODeliveryFeeRequestDto request)
    {
        try
        { 
            // Replace with actual token retrieval logic
            var accessToken = await _OtoApiService.GetAccessTokenAsync();

            var response = await _OtoApiService.CheckOTODeliveryFeeAsync(request, accessToken);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, errorMessage = ex.Message });
        }
    }
    public async Task<IActionResult> shipment()
    {
        return View();
    }


    public async Task<IActionResult> GetPickupLocations(
        [FromQuery] string? minDate,
        [FromQuery] string? maxDate,
        [FromQuery] string? status)
    {
        try
        {
            // Validate date range logic (e.g., maxDate cannot be earlier than minDate)
            if (!string.IsNullOrEmpty(minDate) && !string.IsNullOrEmpty(maxDate) && DateTime.Parse(minDate) > DateTime.Parse(maxDate))
            {
                return BadRequest("minDate cannot be later than maxDate.");
            }
            var accessToken = await _OtoApiService.GetAccessTokenAsync();
            var data = await _OtoApiService.GetPickupLocationList(accessToken,minDate, maxDate, status);
            return Ok(data);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    // Refactor: Remove retry logic from ProcessShipmentAsync, rely on PostAsync retries
    [HttpPost]
    [Route("Backend/orders/ProcessShipment")]
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> ProcessShipment([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            if (createOrderDto == null)
                return BadRequest("Invalid request. Order data is missing.");

            string accessToken = await _OtoApiService.GetAccessTokenAsync();
            string orderResponse = null;
            int orderRetryCount = 0;

            while (orderRetryCount < 3)
            {
                orderResponse = await _OtoApiService.CreateOrderAsync(createOrderDto, accessToken);

                if (!string.IsNullOrWhiteSpace(orderResponse))
                    break;

                orderRetryCount++;
                _logger.LogWarning($"Retry {orderRetryCount} failed to create order.");
                await Task.Delay(1000); // Wait before retrying
            }

            if (string.IsNullOrWhiteSpace(orderResponse))
            {
                _logger.LogError("Failed to create order after 3 attempts.");
                return BadRequest("Failed to create order.");
            }

            // Validate JSON format
            if (IsValidJson(orderResponse))
            {
                var orderResponseData = JsonConvert.DeserializeObject<dynamic>(orderResponse);

                if (orderResponseData.success == true && orderResponseData.otoId != null)
                {
                    string otoId = orderResponseData.otoId?.ToString();
                    string success = orderResponseData.success?.ToString();
                    int orderIds = int.Parse(createOrderDto.orderId);
                    DateTime shippingDate = DateTime.UtcNow.AddHours(3);

                    var updateshipment = await _Orderss.updateshipment(orderIds, otoId, null, shippingDate, success, 4, null);
                    if (updateshipment.isSuccess)
                    {
                        await _OrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, orderIds, 4, updateshipment.Result, null);

                        return Ok(new
                        {
                            Message = "Shipment process completed successfully.",
                            OrderResponse = orderResponse
                        });
                    }
                }

                // Handle failure case from API
                string errorMessage = orderResponseData.otoErrorMessage != null
                    ? orderResponseData.otoErrorMessage.ToString()
                    : "Unknown error from API.";

                return Ok(new
                {
                    Message = errorMessage,
                    OrderResponse = orderResponse
                });
            }
            else
            {
                _logger.LogError($"Invalid JSON response received: {orderResponse}");
                return StatusCode(500, "Received unexpected response format from API.");
            }
        }
        catch (JsonReaderException jex)
        {
            _logger.LogError($"JSON parse error: {jex.Message}");
            return StatusCode(500, "Invalid JSON response received.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unhandled exception: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // Utility function
    private bool IsValidJson(string str)
    {
        str = str.Trim();
        if ((str.StartsWith("{") && str.EndsWith("}")) || (str.StartsWith("[") && str.EndsWith("]")))
        {
            try
            {
                var obj = JToken.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }

     



    //public async Task RunShippingWorkflow(string refreshToken)
    //{
    //    // Step 1: Get Access Token
    //    var accessToken = await _OtoApiService.GetAccessTokenAsync(refreshToken);

    //    // Step 2: Get All Delivery Options
    //    var deliveryOptions = await _OtoApiService.GetDeliveryOptionsAsync(accessToken);
    //    if (deliveryOptions == null || !deliveryOptions.Any())
    //    {
    //        throw new Exception("No delivery options available.");
    //    }

    //    // Step 3: Select a Suitable Delivery Company
    //    var selectedOption = _selectionService.SelectBestDeliveryOption(deliveryOptions);
    //    if (selectedOption == null)
    //    {
    //        throw new Exception("No suitable delivery option found.");
    //    }
    //    Console.WriteLine($"Selected Delivery Company: {selectedOption.CompanyName}, Fee: {selectedOption.Fee}");

    //    // Step 4: Create Pickup Location
    //    var pickupLocationPayload = new { branch_code = "12345", address = "123 Pickup Street", city = "Cairo" };
    //    var pickupLocationResponse = await _OtoApiService.PostAsync("https://api.tryoto.com/rest/v2/createPickupLocation", pickupLocationPayload, accessToken);
    //    var pickupLocation = JsonConvert.DeserializeObject<PickupLocation>(pickupLocationResponse);
    //    Console.WriteLine($"Pickup Location Created: {pickupLocation.PickupLocationCode}");

    //    // Step 5: Create Order
    //    var orderPayload = new
    //    {
    //        order_id = Guid.NewGuid().ToString(),
    //        delivery_option_id = selectedOption.DeliveryOptionId,
    //        pickup_location_code = pickupLocation.PickupLocationCode,
    //        customer_name = "John Doe",
    //        customer_phone = "0123456789",
    //        items = new[] { new { item_id = "ITEM001", quantity = 1, price = 100.0 } }
    //    };
    //    var orderResponse = await _OtoApiService.PostAsync("https://api.tryoto.com/rest/v2/createOrder", orderPayload, accessToken);
    //    var order = JsonConvert.DeserializeObject<OrderResponse>(orderResponse);
    //    Console.WriteLine($"Order Created: {order.OrderId}");

    //    // Step 6: Create Shipment
    //    var shipmentPayload = new { order_id = order.OrderId, shipment_details = new { weight = 5.0, dimensions = "10x10x10" } };
    //    var shipmentResponse = await _OtoApiService.PostAsync("https://api.tryoto.com/rest/v2/createShipment", shipmentPayload, accessToken);
    //    Console.WriteLine("Shipment Created: " + shipmentResponse);

    //    // Step 7: Track Order
    //    var trackPayload = new { order_id = order.OrderId };
    //    var trackingResponse = await _OtoApiService.PostAsync("https://api.tryoto.com/rest/v2/trackOrder", trackPayload, accessToken);
    //    Console.WriteLine("Order Tracking Details: " + trackingResponse);
    //}


}

