using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.Roles;
using Kader.DTOs.Stores;
using Kader.DTOs.Users;
using Kader.DTOs.UsersStores;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Middlewares.Cookies;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.RoleDetail;
using Kader.Services.Implementations.Sms;
using Kader.Services.Implementations.Stores;
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
using System.Text.Json;
using DocumentFormat.OpenXml.InkML;
using System.Security.Claims;

namespace Kader.Areas.Backend.Controllers

{
    [Area("Backend")]

    public class AccountController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IAuthenticateService _userService; private ISmsService _SmsService;
        private IUserRoleService _UserRoleService; private IStoresService _Stores;
        private IUsersStoresService _UsersStores;
        private IRoleDetailService _RoleDetailService;
        private readonly ICookies _cookies; private IKeys _keys;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        
        public AccountController( RoleManager<ApplicationRole> roleManage, ISmsService SmsService, IStoresService StoresService, IUsersStoresService UsersStoresService, IRoleDetailService RoleDetailService, IUserRoleService UserRoleService, IAuthenticateService userService, IHttpContextAccessor httpContextAccessor, ICookies cookies)
        {
            _userService = userService; _cookies = cookies; _Stores = StoresService; _UsersStores = UsersStoresService;
            _roleManager = roleManage; _UserRoleService = UserRoleService;
            _RoleDetailService = RoleDetailService; _SmsService = SmsService;
            _httpContextAccessor = httpContextAccessor; _keys = new Keys();
        }

        public IActionResult Login()
        {


            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Catogry", "Catogry");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl)
       {
            try
            {
                ReturnDto<LoginUserDto> doLogin;
                try
                {
                doLogin = await _userService.Login(model.userName, model.Userpass, _keys.TypeUser_backend(), HttpContext);


                if (doLogin.isSuccess)
                {
                    var val = Newtonsoft.Json.JsonConvert.SerializeObject(doLogin.Result);

                    var isAdded = await _cookies.Add("userSession", val);
                        bool InsideShippingRole = HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.InsideShippingRole());
                        // return RedirectToAction("Catogry", "Catogry");
                        if(InsideShippingRole)
                        {
                            return RedirectToAction("InnerunderShipent", "Orders");
                        }
                        else
                        return RedirectToAction("Index", "Homes");

                    }
                else
                {
                    //return new JsonResult(doLogin.ErrorMsg);

                    return RedirectToAction("Login", "Account");
                }
                }  catch (Exception ex)
            {
                Console.WriteLine("testheb"+ex.Message+ model.userName+ model.Password);
               
            }

        }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            try
            {
                var GetStores = await _Stores.GetStores();
                if (GetStores.isSuccess)
                {
                    ViewBag.Stores = GetStores.Result;
                }

                var GetAllUsers = await _userService.GetAllUsers();
                if (GetAllUsers.isSuccess)
                {
                    var allUsers = GetAllUsers.Result;
                    var filteredUsers = new List<AllUserViewDto>();

                    foreach (var user in allUsers)
                    {
                        var roles = user.Roles;

                        if (roles.Contains("مخزن") || roles.Contains("خدمات شحن") || roles.Contains("مندوب نقل داخلي"))
                        {
                            var userStores = await _UsersStores.GetUserStores(user.Id);
                            if (userStores.isSuccess)
                            {
                                user.UsersStoresDto = userStores.Result;
                                user.StoreId = user.UsersStoresDto.Select(s => s.StoreId ?? 0).ToList();
                            }
                        }

                        filteredUsers.Add(user);
                    }

                    return View(filteredUsers);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AllUserViewDto model, IList<string> Roles)
        {
            try
            {
                model.TypeUser = _keys.TypeUser_backend();

                // Validate required fields based on roles
                if (Roles.Contains("InsideShippingRole") && string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    ModelState.AddModelError("", "يجب إدخال رقم التليفون عند اختيار دور 'مندوب نقل داخلي'.");
                    ViewBag.Stores = (await _Stores.GetStores()).Result;
                    return View(model);
                }

                if ((Roles.Contains("Store") && (model.StoreId == null || !model.StoreId.Any())) ||
                    (Roles.Contains("InsideShippingRole") && (model.StoreId == null || !model.StoreId.Any())) ||
                    (Roles.Contains("ShippingRole") && (model.StoreId == null || !model.StoreId.Any())))
                {
                    ModelState.AddModelError("", "يجب اختيار مخزن على الأقل عند تعيين المستخدم كـ 'مسئول مخزن' أو 'خدمات شحن' أو 'مندوب نقل داخلي'.");
                    ViewBag.Stores = (await _Stores.GetStores()).Result;
                    return View(model);
                }

                // ✅ Step 1: Send OTP Before Saving User
                string phone = model.CountryCode + model.PhoneNumber;
                var otpResponse = await _SmsService.SendOtpAsync(phone);
                if (otpResponse == null || otpResponse.id == 0)
                {
                    ModelState.AddModelError("", "فشل في إرسال رمز التحقق.");
                    return View(model);
                }

                // ✅ Step 2: Store User Data Temporarily in Session Until OTP is Verified
                HttpContext.Session.SetString("PendingUserData", JsonSerializer.Serialize(model));
                HttpContext.Session.SetString("UserRoles", JsonSerializer.Serialize(Roles));
                HttpContext.Session.SetString("MobileNumber", phone);
                HttpContext.Session.SetInt32("OtpId", otpResponse.id);

                return RedirectToAction("VerifyOtp");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string otpCode)
        {
            try
            {
                string phone = HttpContext.Session.GetString("MobileNumber");
                int? otpId = HttpContext.Session.GetInt32("OtpId");

                if (string.IsNullOrEmpty(phone) || otpId == null)
                {
                    ModelState.AddModelError("", "رمز التحقق غير صالح.");
                    return View();
                }

                var verifyResult = await _SmsService.VerifyOtpAsync(phone, otpCode, otpId.Value);
                if (!verifyResult)
                {
                    ModelState.AddModelError("", "رمز التحقق غير صحيح.");
                    return View();
                }

                // ✅ Step 3: Retrieve User Data from Session and Save Account
                var userDataJson = HttpContext.Session.GetString("PendingUserData");
                var rolesJson = HttpContext.Session.GetString("UserRoles");

                if (string.IsNullOrEmpty(userDataJson) || string.IsNullOrEmpty(rolesJson))
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء استعادة بيانات المستخدم.");
                    return View();
                }

                var model = JsonSerializer.Deserialize<AllUserViewDto>(userDataJson);
                var Roles = JsonSerializer.Deserialize<IList<string>>(rolesJson);

                var user = await _userService.Register(_httpContextAccessor.HttpContext, model);
                if (!user.isSuccess)
                {
                    ModelState.AddModelError("", "حدث خطأ أثناء إنشاء الحساب.");
                    return View();
                }

                var userRoles = new List<string>();

                if (Roles.Contains("Admin")) userRoles.Add(_keys.Admin());
                if (Roles.Contains("InsideShippingRole")) userRoles.Add(_keys.InsideShippingRole());
                if (Roles.Contains("Store")) userRoles.Add(_keys.storeRole());
                if (Roles.Contains("ShippingRole")) userRoles.Add(_keys.ShippingRole());
                if (Roles.Contains("UsersRole")) userRoles.Add(_keys.UsersRole());
                if (Roles.Contains("ManagOrderRole")) userRoles.Add(_keys.ManagOrderRole());
                if (Roles.Contains("ReportRole")) userRoles.Add(_keys.ReportRole());
                if (Roles.Contains("MainRole")) userRoles.Add(_keys.MainRole());
                if (Roles.Contains("OtherPagesRole")) userRoles.Add(_keys.OtherPagesRole());
                if (Roles.Contains("ProductRole")) userRoles.Add(_keys.ProductRole());
                if (Roles.Contains("CatogryRole")) userRoles.Add(_keys.CatogryRole());

                await _userService.AddUserToRoles(_httpContextAccessor.HttpContext, user.Result, userRoles);
                if (((Roles.Contains("Store") && (model.StoreId != null)) ||
                   (Roles.Contains("InsideShippingRole") && (model.StoreId != null)) ||
                   (Roles.Contains("ShippingRole") && (model.StoreId != null))))
                {
                  var SaveUsersStores =   await _UsersStores.SaveUsersStores(_httpContextAccessor.HttpContext, model.StoreId, user.Result);
                    if (!SaveUsersStores.isSuccess)
                    {
                        ModelState.AddModelError("", "حدث خطأ أثناء إنشاء الحساب.");
                        return View();
                    }
                }
                    // ✅ Step 4: Clear Session After Successful Registration
                    HttpContext.Session.Remove("PendingUserData");
                HttpContext.Session.Remove("UserRoles");
                HttpContext.Session.Remove("MobileNumber");
                HttpContext.Session.Remove("OtpId");

                return RedirectToAction("Register"); // Redirect to a success page
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            try
            {
                var userResponse = await _userService.GetUserID(userId);
                if (!userResponse.isSuccess || userResponse.Result == null)
                {
                    return NotFound("المستخدم غير موجود.");
                }

                var user = userResponse.Result;

                // Fetch all available stores
                var storesResponse = await _Stores.GetStores();
                if (storesResponse.isSuccess)
                {
                    ViewBag.Stores = storesResponse.Result;
                }

                // Get user roles and store role keys
                var rolesResponse = await _UserRoleService.GetUserRoles(userId);

                // Extract role keys (RoleId or RoleKey, whichever exists)
                var userRoles = rolesResponse.isSuccess
                    ? rolesResponse.Result.Select(r => r.Name).ToList()
                    : new List<string>();

                // Get user's assigned stores and convert List<int?> to List<int>
                var userStoresResponse = await _UsersStores.GetUserStores(userId);
                var userStores = userStoresResponse.isSuccess
                    ? userStoresResponse.Result.Select(s => s.StoreId ?? 0).ToList()
                    : new List<int>();

                var model = new AllUserViewDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userRoles, // Just store the role keys
                    StoreId = userStores // Convert nullable int to int
                };

                return View("EditUser", model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "حدث خطأ أثناء جلب بيانات المستخدم.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] AllUserViewDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Id))
            {
                ViewBag.ErrorMessage = "بيانات غير صالحة.";
                return View(model); // Return the same view with error message
            }

            // 1️⃣ Update User Information
            var userUpdateResult = await _userService.UpdateUserDetailsAsync(model);
            if (!userUpdateResult.isSuccess)
            {
                ViewBag.ErrorMessage = userUpdateResult.ErrorMsg;
                var storesResponse = await _Stores.GetStores();
                if (storesResponse.isSuccess)
                {
                    ViewBag.Stores = storesResponse.Result;
                }
                return View(model);
            }

            // 2️⃣ Update User Roles
            var roleUpdateResult = await _UserRoleService.UpdateUserRolesAsync(model.Id, model.Roles.ToList());
            if (!roleUpdateResult.isSuccess)
            {
                var storesResponse = await _Stores.GetStores();
                if (storesResponse.isSuccess)
                {
                    ViewBag.Stores = storesResponse.Result;
                }
                ViewBag.ErrorMessage = roleUpdateResult.ErrorMsg;
                return View(model);
            }

            // 3️⃣ Update User Stores (Only if roles require it)
            if (model.Roles.Any(r => r == "مخزن" || r == "خدمات شحن" || r == "مندوب نقل داخلي"))
            {
                var storeUpdateResult = await _UsersStores.UpdateUserStoresAsync(HttpContext, model.StoreId, model.Id);
                if (!storeUpdateResult.isSuccess)
                {
                    ViewBag.ErrorMessage = storeUpdateResult.ErrorMsg;
                    var storesResponse = await _Stores.GetStores();
                    if (storesResponse.isSuccess)
                    {
                        ViewBag.Stores = storesResponse.Result;
                    }
                    return View(model);
                }
            }

            ViewBag.SuccessMessage = "تم تحديث المستخدم بنجاح.";
            return RedirectToAction("Register"); // Redirect to Register page on success
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }


        //    return View(model);
        //}
        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            try
            {
                var checkProductByCatID = _userService.DeleteUser(_httpContextAccessor.HttpContext, id).Result;
                if (checkProductByCatID.isSuccess)
                {
                    return Json("تم الحذف بنجاح ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
            }


            return Json("حدث مشكلة");
        }

    }
}
