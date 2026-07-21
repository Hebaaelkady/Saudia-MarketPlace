using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.Orderss;
using Kader.DTOs.Product;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Middlewares.Cookies;
using Kader.Services.Implementations.Address;
using Kader.Services.Implementations.Cities;
using Kader.Services.Implementations.Governorates;
using Kader.Services.Implementations.Neighborhood;
using Kader.Services.Implementations.Order;
using Kader.Services.Implementations.OrderItems;
using Kader.Services.Implementations.ReturnOrders;
using Kader.Services.Implementations.ReturnsOrderItem;
using Kader.Services.Implementations.RoleDetail;
using Kader.Services.Implementations.Sms;
using Kader.Services.Implementations.User;
using Kader.Services.Implementations.UserRoles;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
namespace Kader.Controllers

{ 

    public class AccountController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IAuthenticateService _userService;
        private IUserRoleService _UserRoleService;
        private IOrderssService _OrderService; private ISmsService _SmsService;
        private IOrderItemsService _OrderItemsService;
        private IRoleDetailService _RoleDetailService; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICookies _cookies; private IKeys _keys;
        private IAddressService _AddressService;
        private IGovernoratesService _GovernoratesService;
        private ICitiesService _CitiesService;
        private INeighborhoodService _NeighborhoodService;
        private IReturnOrdersService _ReturnsOrderService;
        private IReturnsOrderItemService _ReturnsOrderItemService;
        public AccountController(RoleManager<ApplicationRole> roleManage, IReturnsOrderItemService ReturnsOrderItemService, ISmsService SmsService, IReturnOrdersService ReturnsOrderService, INeighborhoodService NeighborhoodService, IOrderssService OrderService, IOrderItemsService OrderItemsService, IGovernoratesService GovernoratesService, ICitiesService CitiesService, IRoleDetailService RoleDetailService, IAddressService AddressService, IUserRoleService UserRoleService, IAuthenticateService userService, IHttpContextAccessor httpContextAccessor, ICookies cookies)
        {
            _userService = userService; _cookies = cookies; _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManage; _UserRoleService = UserRoleService; _GovernoratesService = GovernoratesService;
            _CitiesService = CitiesService; _keys = new Keys(); _SmsService = SmsService;
            _OrderItemsService = OrderItemsService; _NeighborhoodService = NeighborhoodService;
            _AddressService = AddressService; _OrderService = OrderService;
            _AddressService = AddressService; _ReturnsOrderService = ReturnsOrderService; _ReturnsOrderItemService = ReturnsOrderItemService;
        }

        public async Task<IActionResult> Login()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("AccountCustomer", "Account"); // Redirect to PageX if authenticated
            //}
           
      

             return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl)
        {
            try
            {
                ReturnDto<LoginUserDto> doLogin;
                try
                {                        // return RedirectToAction("Catogry", "Catogry");

                    doLogin = await _userService.Login(model.userName, model.Userpass, _keys.TypeUser_frontend(), HttpContext);
                    if (doLogin.isSuccess)
                    {
                        var val = Newtonsoft.Json.JsonConvert.SerializeObject(doLogin.Result);
                        var isAdded = await _cookies.Add("user", val);
                        return RedirectToAction("AccountCustomer", "Account");
                    }
                    else
                    {
                        ViewBag.error = doLogin.ErrorMsg;
                        return RedirectToAction("Login", "Account");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }

            }



            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return View(model);
        }

        public  IActionResult  Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AllUserViewDto model)
        {
            try
            {
                model.PhoneNumber = model.Username;
                if (!ModelState.IsValid)
                {
                    return View(model); // Return validation errors
                }

                // Default email if not provided
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    model.Email = "default@example.com";
                }

                // Ensure required fields are provided
                if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                {
                    ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ!";
                    return View(model);
                }
                model.PhoneNumber = model.Username;
                // Send OTP before saving user
                string phone = model.CountryCode + model.Username;
                var otpResponse = await _SmsService.SendOtpAsync(phone);

                if (otpResponse == null || otpResponse.id == 0)
                {
                    ModelState.AddModelError("", "فشل في إرسال رمز التحقق.");
                    return View(model);
                }

                // ✅ Store User Data Temporarily Until OTP is Verified
                HttpContext.Session.SetString("PendingUserData", JsonSerializer.Serialize(model));
                HttpContext.Session.SetString("MobileNumber", phone);
                HttpContext.Session.SetInt32("OtpId", otpResponse.id);

                return RedirectToAction("VerifyCode");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "حدث خطأ غير متوقع: " + ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCode(string otpCode)
        {
            string phone = HttpContext.Session.GetString("MobileNumber");
            int? otpId = HttpContext.Session.GetInt32("OtpId");

            if (otpId == null)
            {
                ModelState.AddModelError("", "رمز التحقق غير موجود أو انتهت صلاحيته. الرجاء التسجيل مرة أخرى.");
                return View();
            }

            // Verify OTP with SMS Service
            bool isOtpValid = await _SmsService.VerifyOtpAsync(phone, otpCode, otpId.Value);
            if (!isOtpValid)
            {
                ModelState.AddModelError("", "رمز التحقق غير صحيح.");
                return View();
            }

            // ✅ OTP Verified → Retrieve Stored User Data
            var userDataJson = HttpContext.Session.GetString("PendingUserData");
            if (string.IsNullOrEmpty(userDataJson))
            {
                ModelState.AddModelError("", "تعذر استرجاع بيانات المستخدم. حاول مرة أخرى.");
                return View();
            }

            var model = JsonSerializer.Deserialize<AllUserViewDto>(userDataJson);
 model.PhoneNumber = model.Username;
            // Assign user type and register user
            model.TypeUser = _keys.TypeUser_frontend();
           
            var user = await _userService.Register(_httpContextAccessor.HttpContext, model);

            if (!user.isSuccess)
            {
                ModelState.AddModelError("", user.ErrorMsg);
                return View();
            }

            // Assign roles to the user
            IList<string> roles = new List<string> { _keys.frontEndUserRole() };
            var val = await _userService.AddUserToRoles(_httpContextAccessor.HttpContext, user.Result, roles);

            if (!val.isSuccess)
            {
                ModelState.AddModelError("", "حدثت مشكلة أثناء إضافة الأدوار.");
                return View();
            }

            // ✅ Clear Session After Successful Registration
            HttpContext.Session.Remove("PendingUserData");
            HttpContext.Session.Remove("MobileNumber");
            HttpContext.Session.Remove("OtpId");

            ViewBag.SuccessMessage = "تم التسجيل بنجاح! يمكنك الآن تسجيل الدخول من خلال <a href='/Account/Login'>هنا</a>.";
            return RedirectToAction("Login", "Account");
        }


        public static bool IsValidSaudiPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            // Saudi mobile and landline regex
            string pattern = @"^(?:\+966|966|0)?5\d{8}$"; // Mobile numbers
            string landlinePattern = @"^(?:\+966|966|0)?(1|2|3|4|6|7|8)\d{7}$"; // Landline numbers

            return Regex.IsMatch(phoneNumber, pattern) || Regex.IsMatch(phoneNumber, landlinePattern);
        }
        public IActionResult AllAddress()
        {
            // ReturnDto<AddressDto> getAddressResult;
            var getAddressResult = _AddressService.GetAddress(_httpContextAccessor.HttpContext).Result;

            if (getAddressResult.isSuccess)
            {
                return View(getAddressResult.Result);
            }
            else
            {
                return View();
            }

        }
        public async Task<IActionResult> AccountCustomer()
        {
            var GetUserInfo = await _userService.GetUser(_httpContextAccessor.HttpContext);
            if (GetUserInfo.isSuccess)
            {
                var getAddressResult = _AddressService.GetActiveAddress(_httpContextAccessor.HttpContext).Result;

                if (getAddressResult.isSuccess)
                {
                    GetUserInfo.Result.ActiveAddressDto = getAddressResult.Result;
                }
                else
                {
                    GetUserInfo.Result.ActiveAddressDto = null;
                }
                return View(GetUserInfo.Result);
            }
            return View();
        }

        public async Task<IActionResult> AddAddress(int? AddressId, AddressDto model)
        {
           
            // Load Governorates for the dropdown
            var getGovernorates = await _GovernoratesService.GetGovernorates(_httpContextAccessor.HttpContext);
            if (getGovernorates.isSuccess)
            {
                ViewBag.GovernoratesId = new SelectList(getGovernorates.Result, "GovernorateId", "GovernorateName");
            }

            var viewModel = new AddressDto();

            if (AddressId != null)
            {
               
                var getAddressResult = await _AddressService.GetSingleAddress(AddressId.Value);
                
                if (getAddressResult.isSuccess)
                {
                    var GetOrderNotMoreThan14DayStatus1And3 = await _OrderService.GetOrderNotMoreThan14DayStatus1And3(_httpContextAccessor.HttpContext);
                    if (GetOrderNotMoreThan14DayStatus1And3.isSuccess&& getAddressResult.Result.IsMain == true)
                    {
                        ViewBag.ErrorMessage = "لا يمكنك تعديل العنوان لأن هناك طلب قيد التنفيذ خلال الـ 14 يوم الماضية.";
                        return View(viewModel);
                    }
                    ViewBag.GovernoratesId = new SelectList(getGovernorates.Result, "GovernorateId", "GovernorateName");
                    viewModel = getAddressResult.Result;
                }
            }

            // Check for null model and validate if necessary fields are filled
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "The model is not valid.");
                return View(viewModel); // Return to the view with the error
            }

            // Validation for required fields
            if (string.IsNullOrEmpty(model.AddressStreet))
            {
                ModelState.AddModelError("AddressStreet", "من فضلك ادخل عنوان الشارع.");
                return View(viewModel);
            }

            if (string.IsNullOrEmpty(model.BuildingNo))
            {
                ModelState.AddModelError("BuildingNo", "من فضلك ادخل رقم المبني.");
                return View(viewModel);
            }

            if (model.NeighborhoodId == null)
            {
                ModelState.AddModelError("City", "من فضلك اختار الحي.");
                return View(viewModel);
            }

            var activeAddresses = await _AddressService.GetActiveAddress(_httpContextAccessor.HttpContext);
            if (activeAddresses.isSuccess)
            {
            if (model.IsMain == true)
            {
                activeAddresses.Result.IsMain = false;
                await _AddressService.SaveAddress(_httpContextAccessor.HttpContext, activeAddresses.Result);
            }
        }
        else
        { model.IsMain ??= true; 
            }
                // Default IsMain to true if null
            model.ShiftDelivary ??= 0; // Default to Morning Shift if null

            var saveAddressResult = await _AddressService.SaveAddress(_httpContextAccessor.HttpContext, model);
            if (saveAddressResult.isSuccess)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :";
                ModelState.Clear();
                return RedirectToAction("AllAddress", "Account");
            }
                else
            ViewBag.ErrorMessage = "حدثت مشكلة !";


            return View(viewModel); // Return the view with the validation errors
        }

        [HttpGet]
        public JsonResult GetCities(int governorateID)
        {
            var getCities =  _CitiesService.GetCities(governorateID).Result;
            if (getCities.isSuccess)
            {
                return Json(new SelectList(getCities.Result, "CityId", "CityName"));
            }
            else
                return Json(new { success = false, message = "Failed to retrieve cities." });
        }
        [HttpGet]
        public JsonResult GetNeighborhood(int CityID)
        {
            var getCities = _NeighborhoodService.GetNeighborhood(CityID).Result;
            if (getCities.isSuccess)
            {
                return Json(new SelectList(getCities.Result, "NeighborhoodId", "Name"));
            }
            else
                return Json(new { success = false, message = "Failed to retrieve cities." });
        }
        public IActionResult OrderDetails(string id)
        {

            if (id == null)
            {
                return View();
            }
            else
            {
                var SaveOrder = _OrderService.GetSingleOrder<Checkout_frontDto>(_httpContextAccessor.HttpContext, id).Result;
                if (SaveOrder.isSuccess)
                {
                    return View(SaveOrder.Result);
                }
       
            return View();}
        }

        public IActionResult AllOrders()
        {
             
            var GetOrderResult = _OrderService.GetOrder<OrderssDto>(_httpContextAccessor.HttpContext).Result;

            if (GetOrderResult.isSuccess)
            {
                return View(GetOrderResult.Result);
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.SuccessMessage = "تاكيد كلمة السر الجديدة غير مطابقة لكلمة السر";
                return View(model);
            }

            var result = await _userService.ChangePasswordAsync(_httpContextAccessor.HttpContext,model);

            if (result.isSuccess && result.Result)
            {
                ViewBag.SuccessMessage = result.ErrorMsg;
                //return RedirectToAction("Index", "Home"); // Redirect to a desired page
            }

            ViewBag.SuccessMessage = "خطا اثناء التغيير";
            return View(model);
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (user, countryCode) = await _userService.FindByMobileNumberAsync(model.MobileNumber);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Mobile number not registered.");
                return View(model);
            }
            string MobileNumber =   model.MobileNumber;
            string fullMobileNumber = countryCode + model.MobileNumber;
            // Send OTP
            var otpResponse = await _SmsService.SendOtpAsync(fullMobileNumber);
            if (otpResponse == null || otpResponse.id == 0)
            {
                ModelState.AddModelError(string.Empty, "Failed to send OTP.");
                return View(model);
            }

            // ✅ Store OTP in Session (Instead of TempData)
            HttpContext.Session.SetString("Mobile", MobileNumber);
            HttpContext.Session.SetString("MobileNumber", fullMobileNumber);
            HttpContext.Session.SetInt32("OtpId", otpResponse.id);

            return RedirectToAction("VerifyOtp");
        }

        // Step 2: Verify OTP
        [HttpGet]
        public IActionResult VerifyOtp()
        {
            var mobileNumber = HttpContext.Session.GetString("MobileNumber");
            var mobile  = HttpContext.Session.GetString("Mobile");
            if (string.IsNullOrEmpty(mobileNumber))
            {
                return RedirectToAction("ForgotPassword");
            }

            return View(new VerifyOtpAndResetPasswordDto { MobileNumber = mobileNumber , Mobile = mobile });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyOtp(VerifyOtpAndResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var otpId = HttpContext.Session.GetInt32("OtpId");
            if (otpId == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            var isValidOtp = await _SmsService.VerifyOtpAsync(model.MobileNumber, model.Otp, otpId.Value);
            if (!isValidOtp)
            {
                ModelState.AddModelError(string.Empty, "Invalid OTP.");
                return View(model);
            }

            // Step 3: Update Password Securely
            var updateResult = await _userService.UpdatePasswordAsync(model.MobileNumber, model.Mobile, model.NewPassword);
            if (!updateResult.isSuccess)
            {
                ModelState.AddModelError(string.Empty, updateResult.ErrorMsg);
                return View(model);
            }
            // ✅ Remove OTP session after successful password reset
            HttpContext.Session.Remove("OtpId");
            HttpContext.Session.Remove("MobileNumber");
            return RedirectToAction("Login", "Account", new { message = "Password changed successfully. Please login." });

           
             
        }


        public IActionResult ReturnOrdersAll()
        {
            try
            {
                var result = _ReturnsOrderService.GetReturnsOrder<AllReturnsOrdersDto>(_httpContextAccessor.HttpContext).Result;

            if (result.isSuccess)
            {
                return View(result.Result); // Pass data to the view
            }
            else
            {
                ViewBag.ErrorMessage = "حدث خطا";
                return View(new List<AllReturnsOrdersDto>()); // Empty list in case of error
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "An error occurred while fetching the orders.";
            Console.WriteLine($"Error: {ex.Message}");
            return View(new List<AllReturnsOrdersDto>()); // Empty list in case of exception
        }

}
        public IActionResult ReturnOrders()
        {

            var GetOrderResult = _OrderService.GetOrderNotMoreThan14Day<OrderssDto>(_httpContextAccessor.HttpContext).Result;

            if (GetOrderResult.isSuccess)
            {
                return View(GetOrderResult.Result);
            }
            else
            {
                return View();
            }

        }
        public async Task<IActionResult> ReturnOrderDetails(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                
                    // Step 2: Pass the order items to GetReturnsOrder to check for existing returns
                    var returnOrderDetails = await _ReturnsOrderService.GetReturnsOrderWithItems(_httpContextAccessor.HttpContext, id );

                    if (returnOrderDetails.isSuccess)
                    {
                        returnOrderDetails.Result.Idordersss = id;
                        return View(returnOrderDetails.Result);
                    }
                 

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReturnOrderDetails(saveReturnsOrdersDto returnsOrderDto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "حدث خطأ أثناء الحفظ. يرجى التحقق من المدخلات.";
                return await ReloadOrderDetailsView(returnsOrderDto.Idordersss);
            }

            // Filter and validate selected items
            var selectedItems = returnsOrderDto.ReturnsOrderItem?.Where(item => item.Selected).ToList();
            if (selectedItems == null || !selectedItems.Any())
            {
                ViewBag.Message = "يجب اختيار منتج واحد على الأقل.";
                return await ReloadOrderDetailsView(returnsOrderDto.Idordersss); // Return the view with the current model
            }

            // Check if any selected item has a null Reason
            if (selectedItems.Any(item => item.Reason == null))
            {
                ViewBag.Message = "يجب تحديد سبب لكل منتج محدد.";
                return await ReloadOrderDetailsView(returnsOrderDto.Idordersss); // Return the view with the current model
            }

            // Update the DTO with selected items
            returnsOrderDto.ReturnsOrderItem = selectedItems;

            // Save the return order
            var saveResult = await _ReturnsOrderService.SaveReturnsOrder(_httpContextAccessor.HttpContext, returnsOrderDto);
            if (saveResult.isSuccess)
            {
                ViewBag.Message = "تم الحفظ بنجاح.";
            }
            else
            {
                ViewBag.Message = "حدث خطأ أثناء الحفظ. حاول مرة أخرى.";
            }

            // Reload the order details for the view
            return await ReloadOrderDetailsView(returnsOrderDto.Idordersss);
        }


        private async Task<IActionResult> ReloadOrderDetailsView(int orderId)
        {
            var returnOrderDetails = await _ReturnsOrderService.GetReturnsOrderWithItems (_httpContextAccessor.HttpContext, orderId);
            if (returnOrderDetails.isSuccess)
            {
                return View(returnOrderDetails.Result);
            }

            // If reloading fails, return an error view or the original view with an error message
            ViewBag.Message = "حدث خطأ أثناء تحميل بيانات الطلب.";
            return View("Error"); // Optional: Replace with a custom error view
        }

    }
}
