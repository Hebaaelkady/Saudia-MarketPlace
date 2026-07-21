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

[Area("Backend")]

    public class MSGController : Controller
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
        public MSGController(ILogger<ApplicationLogs> logger, IHubContext<OrderHub> hubContext, ISmsService SmsService, IUsersStoresService UsersStoresService, IUserRoleService UserRoleService, IStoresService StoresService, IAuthenticateService userService, IOtoApiService OtoApiService, IOrderStatusService OrderStatusService, IOrderssService OrderssService, IUnitsService UnitsService, IProductService ProductService, ICatogryService CatogryService, IColorsService ColorsService, IHttpContextAccessor httpContextAccessor)
        {
        _Units = UnitsService; _UserRole = UserRoleService; _userService = userService; _hubContext = hubContext;
        _OtoApiService = OtoApiService; _Stores = StoresService; _UsersStores = UsersStoresService;
        _httpContextAccessor = httpContextAccessor; _OrderStatusService = OrderStatusService;
        _Product = ProductService; _logger = logger; _SmsService = SmsService;
        _Orderss = OrderssService; _keys = new Keys();
    }





    public async Task<IActionResult> MsgNotSendToUser()
    {
        var GetOrderss = await _Orderss.GetAllOrderNotSendMsgToUser(_httpContextAccessor.HttpContext);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    public async Task<IActionResult> MsgNotSendToDelivery()
    {
        var GetOrderss = await _Orderss.GetAllOrderNotSendMsgToDelivery(_httpContextAccessor.HttpContext);
        if (GetOrderss.isSuccess)
        {
            return View(GetOrderss.Result);
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> sendMsgToDelivery(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "رقم الطلب غير صالح!" });
            }

            var singleOrderResult = await _Orderss.GetSingleOrder<OrderDetailsDto>(_httpContextAccessor.HttpContext, id);

            // Check if the order retrieval was successful
            if (!singleOrderResult.isSuccess || singleOrderResult.Result == null)
            {
                return Json(new { success = false, message = "حدثت مشكلة في جلب بيانات الطلب!" });
            }

            // Build the message
            string message = "لديك طلب رقم " + singleOrderResult.Result.Idordersss +
                             " من مخزن " + singleOrderResult.Result.StoreName +
                             " إلى " + singleOrderResult.Result.AddressDto?.Name;

            string phoneNumber = singleOrderResult.Result.CountryCode + singleOrderResult.Result.UserName;

            var otpResponse = await _SmsService.SendSmsAsync(phoneNumber, message);

            if (otpResponse == null)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء إرسال الرسالة لمندوب التوصيل!" });
            }

            if (otpResponse.code == 1)
            {
                MsgDto c = new MsgDto
                {
                    Idorders = singleOrderResult.Result.Idordersss,
                    MsgToDelivery = true,
                    MsgCofeToUser = singleOrderResult.Result.MsgCofeToUser
                };

                var updateshipment = await _Orderss.updateSms(_httpContextAccessor.HttpContext, c);

                if (updateshipment.isSuccess)
                {
                    return Json(new { success = true, message = "تم إرسال الرسالة والتحديث بنجاح :)!" });
                }
                else
                {
                    return Json(new { success = false, message = "تم إرسال الرسالة لكن لم يتم تحديث حالة الطلب!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "فشل في إرسال الرسالة لمندوب التوصيل!" });
            }
        }
        catch (Exception ex)
        {
            // You can also log ex.Message using a logging framework
            return Json(new { success = false, message = "حدثت مشكلة أثناء الإرسال: " + ex.Message });
        }
    }
    public async Task<IActionResult> sendMsgToUser(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "رقم الطلب غير صالح!" });
            }

            var singleOrderResult = await _Orderss.GetSingleOrder<OrderDetailsDto>(_httpContextAccessor.HttpContext, id);

            // Check if the order retrieval was successful
            if (!singleOrderResult.isSuccess || singleOrderResult.Result == null)
            {
                return Json(new { success = false, message = "حدثت مشكلة في جلب بيانات الطلب!" });
            }

            // Build the message
            string message =  "طلبكم قيد الشحن رقم استلام الطلب هو " + singleOrderResult.Result.OrdersNo; ;

            string phoneNumber = singleOrderResult.Result.CountryCode + singleOrderResult.Result.UserName;

            var otpResponse = await _SmsService.SendSmsAsync(phoneNumber, message);

            if (otpResponse == null)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء إرسال الرسالة لمندوب التوصيل!" });
            }

            if (otpResponse.code == 1)
            {
                MsgDto c = new MsgDto
                {
                    Idorders = singleOrderResult.Result.Idordersss,
                    MsgCofeToUser = true,
                    MsgToDelivery= singleOrderResult.Result.MsgToDelivery
                };

                var updateshipment = await _Orderss.updateSms(_httpContextAccessor.HttpContext, c);

                if (updateshipment.isSuccess)
                {
                    return Json(new { success = true, message = "تم إرسال الرسالة والتحديث بنجاح :)!" });
                }
                else
                {
                    return Json(new { success = false, message = "تم إرسال الرسالة لكن لم يتم تحديث حالة الطلب!" });
                }
            }
            else
            {
                return Json(new { success = false, message = "فشل في إرسال الرسالة لمندوب التوصيل!" });
            }
        }
        catch (Exception ex)
        {
            // You can also log ex.Message using a logging framework
            return Json(new { success = false, message = "حدثت مشكلة أثناء الإرسال: " + ex.Message });
        }
    }

}

