using Kader;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Cart;
using Kader.DTOs.Orderss;
using Kader.DTOs.ReturnOrders;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.Cart;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Order;
using Kader.Services.Implementations.OrderStatus;
using Kader.Services.Implementations.Payment;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.ReturnOrders;
using Kader.Services.Implementations.ReturnsOrderItem;
using Kader.Services.Implementations.ReturnsOrderStatus;
using Kader.Services.Implementations.ShippingCompany;
using Kader.Services.Implementations.Stores;
using Kader.Services.Implementations.Units;
using Kader.Services.Implementations.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Area("Backend")]

    public class ReturnOrdersController : Controller
{
    private readonly ILogger<ApplicationLogs> _logger;
    private IAuthenticateService _userService; private IStoresService _Stores;
    private IOrderssService _Orderss; private IPaymentService _PaymentService;
    private IProductService _Product; private IOtoApiService _OtoApiService;
    private ICartService _CartService; IOrderStatusService _OrderStatusService;
    private IReturnsOrderStatusService _ReturnsOrderStatusService;
    private IHttpContextAccessor _httpContextAccessor;
    private IUnitsService _Units; private IReturnOrdersService _ReturnOrders;
    private IReturnsOrderItemService _ReturnsOrderItemService;
    private IKeys _keys; 
        public ReturnOrdersController(ILogger<ApplicationLogs> logger, IStoresService StoresService, IOrderStatusService OrderStatusService, IReturnsOrderItemService ReturnsOrderItemService, IPaymentService PaymentService, ICartService CartService, IAuthenticateService userService, IOtoApiService OtoApiService, IReturnsOrderStatusService ReturnsOrderStatusService, IOrderssService OrderssService, IUnitsService UnitsService, IProductService ProductService, ICatogryService CatogryService, IReturnOrdersService ReturnOrdersService, IHttpContextAccessor httpContextAccessor)
        {
        _Units = UnitsService; _ReturnOrders = ReturnOrdersService; _userService = userService; _CartService = CartService;
        _OtoApiService = OtoApiService; _ReturnsOrderItemService = ReturnsOrderItemService;
        _httpContextAccessor = httpContextAccessor; _PaymentService = PaymentService;
        _Stores = StoresService;
        _Product = ProductService; _logger = logger; _ReturnsOrderStatusService = ReturnsOrderStatusService;
        _Orderss = OrderssService; _keys = new Keys(); _OrderStatusService = OrderStatusService;
    }
    public async Task<IActionResult> NewReturnOrders()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext,1);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> UnderCheck()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, 3);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> underShipent()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, 4);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> underShipentfromCustomer()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, 17);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> AssignedToStores()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, _keys.AssignToStore());
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> completedWithRefundOrders()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, 10);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> RejectedReturnOrders()
    {
        var GetOrderss = await _ReturnOrders.GetAllReturnsOrderByStatus<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext, 2);
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

            var orderStatusResult = await _ReturnsOrderStatusService.GetOrderStatus(request.Idorders);
            if (!orderStatusResult.isSuccess)
            {
                return Json(new { success = false, message = "Failed to fetch order status!" });
            }

            var statusCheck = _keys.AssignToStore();
            var statusExists = orderStatusResult.Result.Any(k => k.StatusId == statusCheck);

            if (!statusExists)
            {
                request.Statusid = statusCheck;
                var updateStatusResult = await _ReturnOrders.update(_httpContextAccessor.HttpContext, request);
                if (!updateStatusResult.isSuccess)
                {
                    return Json(new { success = false, message = "Failed to update order!" });
                }

                var saveOrderStatus = await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, request.Idorders, statusCheck, request.StoreId);
                if (!saveOrderStatus.isSuccess)
                {
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

    public async Task<IActionResult> ReturnOrdersDetails(int id)
    {
        if (id == 0)
        {
            return View();
        }
        var GetStores = await _Stores.GetStores();
        if (GetStores.isSuccess)
        {
            ViewBag.Stores = GetStores.Result;
        }
        var singleOrderResult = await _ReturnOrders.GetSingleReturnsOrder<ReturnOrdersDetailsDto>(_httpContextAccessor.HttpContext, id);
        if (!singleOrderResult.isSuccess)
        {
            return View();
        }

        var roleClaims = User.Claims
                          .Where(c => c.Type == ClaimTypes.Role)
                          .Select(c => c.Value)
                          .ToList();
        if (roleClaims.Count() != 0)
        {
            if (roleClaims.Contains(_keys.Admin()))
            {
                ViewBag.IsAdmin = 1;
            }
            else if (roleClaims.Contains(_keys.storeRole()))
            {
                var orderStatusResult = await _ReturnsOrderStatusService.GetOrderStatus(id);
                if (!orderStatusResult.isSuccess)
                {
                    return View("Error");
                }

                var statusCheck = _keys.status_check();
                var statusExists = orderStatusResult.Result.Any(k => k.StatusId == statusCheck);
                if (!statusExists)
                {
                    var updateStatusResult = await _ReturnOrders.updateStatus(_httpContextAccessor.HttpContext, statusCheck, singleOrderResult.Result.ReturnsOrderId);
                    if (updateStatusResult.isSuccess)
                    {
                        await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, singleOrderResult.Result.ReturnsOrderId, statusCheck, singleOrderResult.Result.StoreID);
                    }
                }
                ViewBag.IsAdmin = 0;
            }
        }

        if (singleOrderResult.Result.StatusId == 1|| singleOrderResult.Result.StatusId == 3 )
        {
            var accessToken = await _OtoApiService.GetAccessTokenAsync();
            string ids = id.ToString();
            var deliveryOptions = await _OtoApiService.GetShipmentDetailsAsync(accessToken, ids);

            var orderResponseData = JsonConvert.DeserializeObject<ApiShippingorderStatus>(deliveryOptions);
            if (orderResponseData.status != null)
            {
                singleOrderResult.Result.ApiShippingorderStatus = orderResponseData;
            }
        }
            return View(singleOrderResult.Result);
        
    } 

  
    [HttpPost]
    public async Task<IActionResult> ProcessShipment([FromBody] ReturnsShipmentDto createOrderDto)
    {
        if (createOrderDto == null)
        {
            return Ok(new { Message = "Invalid input data." });
        }

        try
        {
            if(createOrderDto.AcceptReject==false)
            {
              //  int statusReject = 2;
                createOrderDto.AcceptReturnsOrdersDto.StatusID = 2;
                var updateShipment = await _ReturnOrders.Update(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId);

                if (updateShipment.isSuccess)
                {
                    await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId, 2, updateShipment.Result);
                    return Ok(new
                    {
                        Message = "مرفوض",
                        //OrderResponse = orderResponse
                    });
                }
                return BadRequest(new
                {
                    Message = "يوجد مشكلة اثناء عملية الشحن"
                });
            }
            else
            {

           
            string accessToken = await _OtoApiService.GetAccessTokenAsync();
                // createOrderDto.UpdateOrderStatus.date = DateTime.Now;
                 string orderResponse = await RetryAsync(() => _OtoApiService.Update_Order_Status(createOrderDto.UpdateOrderStatus, accessToken));
               // string orderResponse = await RetryAsync(() => _OtoApiService.UpdateOrderStatusAsync( accessToken));

                if (string.IsNullOrEmpty(orderResponse))
            {
                _logger.LogError("Failed to create order after multiple attempts.");
                return Ok(new { Message = "يوجد مشكلة اثناء عملية الشحن." });
            }
             
            string shipmentResponse = await RetryAsync(() => _OtoApiService.createReturnShipment(createOrderDto.createReturnShipment, accessToken));
           // "{\"msg\":\"Return shipment request is received\",\"success\":true}"
            if (!string.IsNullOrEmpty(shipmentResponse))
            {
                var orderResponseData = JsonConvert.DeserializeObject<dynamic>(shipmentResponse);
                if ((bool)orderResponseData.success)
                {
                    int orderIds = int.Parse(createOrderDto.createReturnShipment.orderId);

                        createOrderDto.AcceptReturnsOrdersDto.StatusID = 4;
                        createOrderDto.AcceptReturnsOrdersDto.ShippingReturnotoMessge = orderResponseData.msg;
                        createOrderDto.AcceptReturnsOrdersDto.ShippingReturnBool = orderResponseData.success;
                        createOrderDto.AcceptReturnsOrdersDto.ShippingDate = DateTime.UtcNow.AddHours(3);
                        var updateShipment = await _ReturnOrders.Update(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId);

                        if (updateShipment.isSuccess)
                        {
                            await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId, 4, updateShipment.Result);
                            return Ok(new
                            {
                                Message = "تم عمل طلبية الشحن بنجاح.",
                                //OrderResponse = orderResponse
                            });
                        }
                         
                }
            }

            return Ok(new { Message = "يوجد مشكلة اثناء عملية الشحن." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing shipment.");
            return Ok(new { Message = "يوجد مشكلة اثناء عملية الشحن" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> ProcessShipment1([FromBody] ReturnsShipmentDto createOrderDto)
    {
        if (createOrderDto == null)
        {
            return Ok(new { Message = "Invalid input data." });
        }

        try
        {
            if (createOrderDto.AcceptReject == false)
            {
                //  int statusReject = 2;

                createOrderDto.AcceptReturnsOrdersDto.ShippingDate = DateTime.UtcNow.AddHours(3);
                createOrderDto.AcceptReturnsOrdersDto.StatusID = 2;
                var updateShipment = await _ReturnOrders.Update(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId);

                if (updateShipment.isSuccess)
                {
                    await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId, 2, updateShipment.Result);
                    return Ok(new
                    {
                        Message = "مرفوض",
                        //OrderResponse = orderResponse
                    });
                }
                return BadRequest(new
                {
                    Message = "يوجد مشكلة اثناء عملية الشحن"
                });
            }
            else
            {
                createOrderDto.AcceptReturnsOrdersDto.ShippingDate = DateTime.UtcNow.AddHours(3);
                createOrderDto.AcceptReturnsOrdersDto.StatusID = 17;
                var updateShipment = await _ReturnOrders.Update(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId);

                if (updateShipment.isSuccess)
                {
                    
                     var o=   await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, createOrderDto.AcceptReturnsOrdersDto.ReturnsOrderId, 17, updateShipment.Result);
                    if (updateShipment.isSuccess)
                    {
                        return Ok(new
                        {
                            Message = "تم",
                            //OrderResponse = orderResponse
                        });
                    }
                    else return Ok(new
                    {
                        Message = "خطا",
                        //OrderResponse = orderResponse
                    });
                }
                return BadRequest(new
                {
                    Message = "يوجد مشكلة  "
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing shipment.");
            return Ok(new { Message = "يوجد مشكلة اثناء عملية الشحن" });
        }
    }

    private async Task<string> RetryAsync(Func<Task<string>> action, int maxRetries = 3, int delayMilliseconds = 1000)
    {
        int retryCount = 0;
        while (retryCount < maxRetries)
        {
            string result = await action();
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            retryCount++;
            await Task.Delay(delayMilliseconds);
        }

        return null;
    }

    [HttpGet]
    public async Task<IActionResult> RefundMoney(int id)
    {
        var refundRequest = await _ReturnOrders.GetSingleReturnsOrder<RefundRequestDto>(_httpContextAccessor.HttpContext, id);
        ViewBag.id = id;
        if (refundRequest.isSuccess)
        {
            return View(refundRequest.Result);
        }

        ViewBag.Error = "Failed to fetch refund request details.";
        return View(); // Render an empty view with error message
    }

    [HttpPost]
    public async Task<IActionResult> RefundMoney(RefundRequestDto refundRequest,int id,int p_id)
    {
        // Validate the model
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Invalid data provided. Please correct the errors and try again.";
            return View(refundRequest);
        }

        try
        {
            // Convert amount to the smallest currency unit (e.g., cents, halala)
            refundRequest.Amount = (int)(refundRequest.Amount * 100);
            ViewBag.id = id;
            // Call the refund API service
            var result = await _CartService.CreateRefundAsync(refundRequest);

            if (result.IsSuccess) { 
                // Populate missing fields
                if (string.IsNullOrEmpty(result.Id))
                {
                    result.Id = refundRequest.PaymentId;
                }
                result.RefundedAt = DateTime.UtcNow.AddHours(3);

                // Update payment details
                var UpdatePayment =await   _PaymentService.UpdatePayment(_httpContextAccessor.HttpContext, result , p_id);
                if(UpdatePayment.isSuccess)
                {
                    var ReturnsOrderItems = await _ReturnsOrderItemService.GetReturnsOrderItems(id);
                    if (ReturnsOrderItems.isSuccess)
                    {
                        var isStockDecreased = await _Product.IncreaseQuantity(ReturnsOrderItems.Result);
                        if (isStockDecreased.isSuccess)
                        { }

                    }
                var updateStatusResult = await _ReturnOrders.updateStatus(_httpContextAccessor.HttpContext, 10, id);
                if (updateStatusResult.isSuccess)
                {
                    var SaveOrderStatus =await _ReturnsOrderStatusService.SaveOrderStatus(_httpContextAccessor.HttpContext, id, 10, updateStatusResult.Result);
                if(SaveOrderStatus.isSuccess)
                    {
                        ViewBag.Message = "تم استرجاع المبلغ بنجاح!";
                        return RedirectToAction("completedWithRefundOrders", "ReturnOrders" );
                    }
                }
             }
            }
            else
            {
                ViewBag.Message = result.Message1;
            }
                // Fetch updated refund request data
                var refundRequestUpdated = await _ReturnOrders.GetSingleReturnsOrder<RefundRequestDto>(_httpContextAccessor.HttpContext, id);

               // ViewBag.Message = "Refund processed successfully!";
                return View(refundRequestUpdated.Result);
            }
            
        
        catch (Exception ex)
        {
            // Handle unexpected exceptions
            ViewBag.Error = $"An unexpected error occurred: {ex.Message}";
            return View(refundRequest);
        }
    }

}

