using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.GetInsideUserViewByDateRange;
using Kader.DTOs.InsideUserView;
using Kader.DTOs.Product;
using Kader.DTOs.Roles;
using Kader.DTOs.Store_shippingUsersReportByDateRange;
using Kader.DTOs.Stores;
using Kader.DTOs.StoreShippingUsersReport;
using Kader.DTOs.Users;
using Kader.DTOs.UsersStores;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Middlewares.Cookies;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.GetInsideUserViewByDateRange;
using Kader.Services.Implementations.InsideUserView;
using Kader.Services.Implementations.Order;
using Kader.Services.Implementations.OrderItems;
using Kader.Services.Implementations.OrderStatus;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.RoleDetail;
using Kader.Services.Implementations.Store_shippingUsersReportByDateRange;
using Kader.Services.Implementations.Stores;
using Kader.Services.Implementations.StoreShippingUsersReport;
using Kader.Services.Implementations.User;
using Kader.Services.Implementations.UserRoles;
using Kader.Services.Implementations.UsersStores;


//using Kader.Services.Implementations.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Spire.Pdf.General.Render.Decode.Jpeg2000.j2k.wavelet.synthesis;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kader.Areas.Backend.Controllers

{
    [Area("Backend")]

    public class ReportsController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IAuthenticateService _userService;
        private IStore_shippingUsersReportByDateRangeService _Store_shippingUsersReportByDateRangeService;
        private IOrderItemsService _OrderItemsService;
        private IInsideUserViewService _InsideUserViewService;
        private IUserRoleService _UserRoleService; private IStoresService _Stores;
 
        private IUsersStoresService _UsersStores; private IGetInsideUserViewByDateRangeService _GetInsideUserViewByDateRange;
        private IRoleDetailService _RoleDetailService; private IOrderssService _Orderss;
        private readonly ICookies _cookies; private IKeys _keys; IOrderStatusService _OrderStatusService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        private IProductService _Product;
        public ReportsController( RoleManager<ApplicationRole> roleManage, IStore_shippingUsersReportByDateRangeService Store_shippingUsersReportByDateRangeService, IGetInsideUserViewByDateRangeService GetInsideUserViewByDateRangeService, IStoreShippingUsersReportService StoreShippingUsersReportService, IInsideUserViewService InsideUserViewService, IProductService ProductService, IOrderItemsService OrderItemsService, IOrderStatusService OrderStatusService, IOrderssService OrderssService, IStoresService StoresService, IUsersStoresService UsersStoresService, IRoleDetailService RoleDetailService, IUserRoleService UserRoleService, IAuthenticateService userService, IHttpContextAccessor httpContextAccessor, ICookies cookies)
        {
            _userService = userService; _cookies = cookies; _Stores = StoresService; _Store_shippingUsersReportByDateRangeService = Store_shippingUsersReportByDateRangeService;
            _UsersStores = UsersStoresService; _GetInsideUserViewByDateRange = GetInsideUserViewByDateRangeService;
            _InsideUserViewService = InsideUserViewService;
            _roleManager = roleManage; _UserRoleService = UserRoleService; _OrderStatusService = OrderStatusService;
            _RoleDetailService = RoleDetailService; _Orderss = OrderssService;
            _OrderItemsService = OrderItemsService;
            _httpContextAccessor = httpContextAccessor; _keys = new Keys(); _Product = ProductService;
        }
        public async Task<IActionResult> TopProductBuy()
        {
            var product = await _OrderItemsService.GetOrderItems();

            if (product.isSuccess)
            {
                return View(product.Result);
            }
            return View();
        }
        public async Task<IActionResult> UserPhone()
        {
           var GetUsersFront = await _userService.GetUsersFront();
            if (GetUsersFront.isSuccess)
            {
                return View(GetUsersFront.Result);
            }
                return View();
        }
       


        public IActionResult StoresAccountMoniter()
        {
            return View(); // Just load the page without calling the database
        }

        [HttpGet("StoresAccountMoniterData")]
        public async Task<IActionResult> StoresAccountMoniterData(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate == null || toDate == null)
                {
                    return Json(new { success = false, message = "Please select a date range." });
                }

                var getInsideUserView = await _Store_shippingUsersReportByDateRangeService.GetAllQuery(fromDate.Value, toDate.Value);

                if (!getInsideUserView.isSuccess || getInsideUserView.Result == null)
                {
                    return Json(new { success = false, message = "No data found.", data = new List<Store_shippingUsersReportByDateRangeDto>() });
                }

                return Json(new { success = true, data = getInsideUserView.Result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InsideDeliveryData: {ex}");
                return Json(new { success = false, message = "An error occurred.", error = ex.Message });
            }
        }

        public IActionResult InsideDelivery()
        {
            return View(); // Just load the page without calling the database
        }

        [HttpGet("InsideDeliveryData")]
        public async Task<IActionResult> InsideDeliveryData(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (fromDate == null || toDate == null)
                {
                    return Json(new { success = false, message = "Please select a date range." });
                }

                var getInsideUserView = await _GetInsideUserViewByDateRange.GetAllEvalutedByMwghInKaderQuery(fromDate.Value, toDate.Value);

                if (!getInsideUserView.isSuccess || getInsideUserView.Result == null)
                {
                    return Json(new { success = false, message = "No data found.", data = new List<GetInsideUserViewByDateRangeDto>() });
                }

                return Json(new { success = true, data = getInsideUserView.Result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InsideDeliveryData: {ex}");
                return Json(new { success = false, message = "An error occurred.", error = ex.Message });
            }
        }

    }
}
