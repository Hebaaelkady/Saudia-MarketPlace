
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Kader.DTOs.Users;
using Kader.DTOs;
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.Data.DataAccessLayer;
using AutoMapper;
using Kader.Services.Utilities.Mappers;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Kader.Services.Implementations.Catogry;
using System.Data.Entity;
using Kader.Data.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using Kader.DTOs.Cart;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using BCrypt.Net;

using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Crypto.Generators;
using Kader.DTOs.Roles;
using Kader.Services.Implementations.UsersStores;
using Kader.DTOs.Address;
using Microsoft.Extensions.Caching.Distributed;

namespace Kader.Services.Implementations.User
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly HttpClient _httpClient;
        private IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ICatogryService _Catogry; private IKeys _keys; private IUsersStoresService _UsersStores;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, string> _otpStore = new Dictionary<string, string>();
        private readonly IDistributedCache _cache;
        private IMapper _mapper;
        public AuthenticateService(HttpClient httpClient, IDistributedCache cache, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, ICatogryService CatogryService,
            RoleManager<ApplicationRole> roleManager, IUsersStoresService UsersStoresService,
            IConfiguration configuration, IJwtFactory jwtFactory)
        {
            _httpClient = httpClient; _UsersStores = UsersStoresService;
            _jwtFactory = jwtFactory; _cache = cache;
            _unitOfWork = unitOfWork; _Catogry = CatogryService;
            _userManager = userManager;
            _roleManager = roleManager; _keys = new Keys();
            _configuration = configuration; _mapper = ObjectMapper.Mapper;
        }
        public async Task<string> GetTokenAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://jinaapi.auditor.sa/Login");
            var content = new StringContent(JsonConvert.SerializeObject(new { username = "Api", password = "123456" }), Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await _httpClient.SendAsync(request);
            try
            {
            response.EnsureSuccessStatusCode();
 }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseDto>(responseContent);
            Console.Write("test"+tokenResponse.ResponseKey);
            return tokenResponse.ResponseKey;
        }


        public async Task<ReturnDto<LoginUserDto>> Login(string userName, string password, int TypeUser, HttpContext context)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null   && user.TypeUser == TypeUser)
                { 
                    bool CheckPasswordAsync = await _userManager.CheckPasswordAsync(user, password);
                if (   CheckPasswordAsync && user.TypeUser == TypeUser)
                {
                    List<string> distinctPaths = new List<string>();

                    var personRoles = await _userManager.GetRolesAsync(user);

                    // var role = await _roleManager.FindByNameAsync(personRoles[0]);

                    var rolesIds = new List<string>();
                        var Stores = new List<int>(); 
                        foreach (var roleName in personRoles)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            rolesIds.Add(role.Id);
                          
                        }
                    }
                        var GetUserStores = await _UsersStores.GetUserStores(user.Id);
                        if (GetUserStores != null && GetUserStores.isSuccess)
                        {
                            var GetUserStore = GetUserStores.Result.Select(k => k.StoreId).Where(s => s.HasValue).Select(s => s.Value).ToList();
                            if (GetUserStore != null)
                            {
                                Stores.AddRange(GetUserStore);

                            }
                             
                        }
                        if (rolesIds.Count != 0)
                        {
                        try
                        {
                            var rolesIdsList = rolesIds.ToList();
                            var distinctPaths2 = await _unitOfWork.RoleDetail.FindAsync(rd => rolesIdsList.Contains(rd.RoleId));
                            distinctPaths = distinctPaths2.Select(d => d.Url).Distinct().ToList();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            return null;
                        }
                    }
                    else
                        return new ReturnDto<LoginUserDto>(false, null, "حدث خطا");
                    try
                    {

                        string distinctPathsJson = Newtonsoft.Json.JsonConvert.SerializeObject(distinctPaths);
                        var claims = new List<Claim>
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.UserName.ToString()),
                         new Claim("TypeUser", user.TypeUser.ToString()),
                           new Claim("FirstName", user.FirstName.ToString()),
                        new Claim("Paths", distinctPathsJson),
                    };

                        foreach (var roleId in rolesIds)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roleId));

                        }
                        foreach (var actIDpiCattype in Stores)
                        {
                                claims.Add(new Claim("Stores", actIDpiCattype.ToString()));
                            }
                            
                            var encToken = await _jwtFactory.Authorize(user, context, claims.ToArray());
                        if (encToken != null)
                        {
                            var result = new LoginUserDto()
                            {
                                AccessToken = encToken.AccessToken,
                                JwToken = encToken.token,
                                ExpireIn = DateTime.UtcNow.AddHours(7).AddSeconds(encToken.ExpiresIn)
                            };
                            var userIdClaim = claims.FirstOrDefault(c => c.Type == "Id");
                            Console.WriteLine($"UserId claim type: {userIdClaim.Type}, claim value: {userIdClaim.Value}");
                            return new ReturnDto<LoginUserDto>(true, result, string.Empty);
                        }
                        else
                        {
                            return new ReturnDto<LoginUserDto>(false, null, "حدث خطا");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                }
                    else
                        return new ReturnDto<LoginUserDto>(false, null, "كلمة السر غير صحيحة");
                } else
                    return new ReturnDto<LoginUserDto>(false, null, "المستخدم غير موجود");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

           // return new ReturnDto<LoginUserDto>(false, null, "Authorization Error!");
        }

        [HttpPost]
        public async Task<ReturnDto<string>> Register(HttpContext context, AllUserViewDto model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new ReturnDto<string>(false, null, "مستخدم موجود !");
            else
            {

                ApplicationUser user = new ApplicationUser()
                {

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,
                    IsDelete = false,
                    
                    CountryCode = model.CountryCode,
                    TypeUser = model.TypeUser,

                };
                if (model.TypeUser == _keys.TypeUser_backend())
                {
                    user.Email = model.Username + "@gmail.com";
                    user.PhoneNumber = model.PhoneNumber;
                }
                else
                {
                    user.PhoneNumber = model.Username;
                }
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                        return new ReturnDto<string>(false, null, string.Empty);
                    else
                        return new ReturnDto<string>(true, user.Id, "تم الحفظ !");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }


            }
        }

        private bool RoleExistsAsync(string roleName, int? catTypeID)
        {
            return  _roleManager.Roles
                .Any(r => r.Name == roleName && r.CatTypeID == catTypeID);
        }

        [HttpPost]
        public async Task<ReturnDto<string>> AddRoles(HttpContext context, string roleName, int? catTypeID, int? catIDaPI)
        {
            if (catTypeID == null)
            {
                return new ReturnDto<string>(false, string.Empty, "CatTypeID cannot be null.");
            }

            // Directly call the private RoleExistsAsync method
            var roleExists =  RoleExistsAsync(roleName, catTypeID.Value);

            if (!roleExists)
            {
                var role = new ApplicationRole(roleName, catTypeID, catIDaPI);

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return new ReturnDto<string>(true, role.Id, string.Empty);
                }

                return new ReturnDto<string>(false, string.Empty, "Failed to create role.");
            }
            else
            {
                return new ReturnDto<string>(false, string.Empty, "Role already exists.");
            }
        }
        [HttpPost]
        public async Task<ReturnDto<bool>> DeleteUser(HttpContext context, string id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var role = await _userManager.FindByIdAsync(id);
                if (role != null)
                {
                    role.IsDelete = true;
                    role.DeletedDate = DateTime.UtcNow.AddHours(3);
                    role.DeletedBy = stringUserId;
                    var updateResult = await _userManager.UpdateAsync(role);

                    if (updateResult.Succeeded)
                    {
                        return new ReturnDto<bool>(true, true, string.Empty);
                    }
                }
                return new ReturnDto<bool>(false, false, "exist!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }


        [HttpPost]
        public async Task<ReturnDto<bool>> AddUserToRoles(HttpContext context, string userid, IList<string> roleName)
        {
            var user = await _userManager.FindByIdAsync(userid);
            IdentityResult t = null; // Initialize t to null
            if (user == null)
                return new ReturnDto<bool>(false, false, "User not found!");
            try
            {
                foreach (var y in roleName)
                {
                    var role = await _roleManager.FindByIdAsync(y);
                    if (role != null)
                    {
                        t = await _userManager.AddToRoleAsync(user, role.Name);
                        if (!t.Succeeded) // If adding to role fails, return immediately
                            return new ReturnDto<bool>(false, false, "Failed to add role!");
                    }
                    else
                    {
                        return new ReturnDto<bool>(false, false, "Role not found!");
                    }
                }
                // After the loop, check if t is not null and succeeded
                if (t != null && t.Succeeded)
                    return new ReturnDto<bool>(true, true, "تم الحفظ بنجاح");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ReturnDto<bool>(false, false, "An exception occurred!");
            }
            // If none of the roles were found or added, return a failed result
            return new ReturnDto<bool>(false, false, "Failed to add roles!");
        }
        public async Task<ReturnDto<List<AllUserViewDto>>> GetAllUsers()
        {
            try
            {
                var usersWithRoles = new List<AllUserViewDto>();
                // Fetch all the Users
                var users = _userManager.Users.Where(k => k.IsDelete == false && k.TypeUser == _keys.TypeUser_backend()).ToList();

                foreach (var user in users)
                {
                    // For each user, fetch their roles
                    var roles = await _userManager.GetRolesAsync(user);
                    usersWithRoles.Add(new AllUserViewDto
                    {
                        Username = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        CountryCode = user.CountryCode,
                        Id = user.Id,
                        Roles = roles
                    });
                }

                var map = _mapper.Map<List<AllUserViewDto>>(usersWithRoles);
                return new ReturnDto<List<AllUserViewDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
                return new ReturnDto<List<AllUserViewDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<UserPhoneDto>>> GetUsersFront()
        {
            try
            {
              var getData =  _userManager.Users.Where(k => k.IsDelete == false && k.TypeUser == _keys.TypeUser_frontend()).ToList();
                var map = _mapper.Map<List<UserPhoneDto>>(getData);
                return new ReturnDto<List<UserPhoneDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<UserPhoneDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<AllUserViewDto>> GetUser(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var getData = _userManager.Users.Where(k => k.IsDelete == false && k.Id == stringUserId).FirstOrDefault();

                //   var getData = (await _unitOfWork.UserPersonSp.GetList(new string[] { id.ToString(), "NULL", "0", "1" })).FirstOrDefault();
                if (getData == null) return new ReturnDto<AllUserViewDto>(false, null, "user not found!");
                var map = _mapper.Map<AllUserViewDto>(getData);
                return new ReturnDto<AllUserViewDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<AllUserViewDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<AllUserViewDto>> GetUserID(string id)
        {
            try
            { 
                var getData = _userManager.Users.Where(k => k.IsDelete == false && k.Id == id).FirstOrDefault();

                 if (getData == null) return new ReturnDto<AllUserViewDto>(false, null, "user not found!");
                var map = _mapper.Map<AllUserViewDto>(getData);
                return new ReturnDto<AllUserViewDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<AllUserViewDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> UpdateUserDetailsAsync(AllUserViewDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return new ReturnDto<bool>(false, false, "المستخدم غير موجود.");
                }

                // Update user details
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                //user.PhoneNumber = model.PhoneNumber;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ReturnDto<bool>(false, false, "فشل تحديث بيانات المستخدم.");
                }

                return new ReturnDto<bool>(true, true, "تم تحديث المستخدم بنجاح.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, "حدث خطأ أثناء تحديث المستخدم.");
            }
        }
        public async Task<ReturnDto<bool>> ChangePasswordAsync(HttpContext context,ChangePasswordDto model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;


            model.Id =  stringUserId;
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return new ReturnDto<bool>(false, false, "User not found.");
            }

            // ChangePasswordAsync attempts to change the user's password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                return new ReturnDto<bool>(true, true, "Password changed successfully.");
            }

            var errors = string.Join("; ", changePasswordResult.Errors.Select(e => e.Description));
            return new ReturnDto<bool>(false, false, $"Password change failed: {errors}");
        }

        public async Task<ReturnDto<int>> SaveCustomerApiDtoApi(string token, CustomerApiDto order)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ReturnDto<int>(false, 0, "Token is null or empty");
            }

            if (order == null)
            {
                return new ReturnDto<int>(false, 0, "Order is null");
            }

            var url = $"https://jinaapi.auditor.sa/InsertCustomer?name={Uri.EscapeDataString(order.name)}&mobile={Uri.EscapeDataString(order.mobile)}&email={Uri.EscapeDataString(order.email)}";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new ReturnDto<int>(false, 0, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReturnDto<int>>(responseContent);
                var jsonResponse = JObject.Parse(responseContent);
                bool success = jsonResponse["success"].Value<bool>();
                int id = success ? jsonResponse["ID"].Value<int>() : 0;
                string message = jsonResponse["Msg"].Value<string>();
                if (result != null && result.isSuccess)
                {
                    return new ReturnDto<int>(true, id, "success to save customer");
                }
                else
                {
                    return new ReturnDto<int>(false, 0, "Failed to save customer");
                }
            }
            catch (HttpRequestException ex)
            {
                return new ReturnDto<int>(false, 0, $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ReturnDto<int>(false, 0, $"Unexpected error: {ex.Message}");
            }
        }

        public    ReturnDto<bool>  checkExist(string username)
        {
             
            try
            {

                var getData =   _userManager.Users.Where(o => o.IsDelete == false && o.UserName==username).FirstOrDefault();
                if (getData== null)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
                return new ReturnDto<bool>(false, false, "err");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }



        public async Task<(ApplicationUser user, string CountryCode)> FindByMobileNumberAsync(string mobileNumber)
        {
            var user = _userManager.Users
    .Where(u => u.PhoneNumber == mobileNumber)
    .Select(u => new { u, u.CountryCode })
    .FirstOrDefault(); // ✅ Works with EF6 (synchronous)


            if (user == null)
                return (null, null);

            return (user.u, user.CountryCode);
        }


        public string GenerateOtpForMobile(string mobileNumber)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStore[mobileNumber] = otp;
            return otp;
        }

        public bool ValidateOtp(string mobileNumber, string otp)
        {
            return _otpStore.TryGetValue(mobileNumber, out var storedOtp) && storedOtp == otp;
        }

        public async Task<ReturnDto<bool>> ResetPasswordAsync(string mobileNumber, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(mobileNumber);
            if (user == null)
            {
                return new ReturnDto<bool>(false, false, "User not found.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _userManager.UpdateAsync(user);
            return new ReturnDto<bool>(true, true, "Password changed successfully.");
        }

        public async Task StoreOtpAsync(string mobileNumber, int otpId)
        {
            await _cache.SetStringAsync($"OTP_{mobileNumber}", otpId.ToString(), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public async Task<int?> GetStoredOtpIdAsync(string mobileNumber)
        {
            var otpIdStr = await _cache.GetStringAsync($"OTP_{mobileNumber}");
            return int.TryParse(otpIdStr, out int otpId) ? (int?)otpId : null;
        }


        public async Task<ReturnDto<List<GetCatogryByRoleDto>>> GetCatogryByRole(HttpContext context, int? type)

        {
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            try
            {
                var getData = _roleManager.Roles
            .Where(r => roleClaims.Contains(r.Id))
            .ToList();
                if (getData != null)
                {
                      var map = _mapper.Map<List<GetCatogryByRoleDto>>(getData);
                    return new ReturnDto<List<GetCatogryByRoleDto>>(true, map, string.Empty);
                }
                else
                    return new ReturnDto<List<GetCatogryByRoleDto>>(false, null, "not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<GetCatogryByRoleDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> UpdatePasswordAsync(string mobileNumber, string mobile, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(mobile);
            
            if (user == null)
            {
                return new ReturnDto<bool>(false, false, "User not found.");
            }

            // Generate a password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                return new ReturnDto<bool>(false, false, $"Failed to update password: {errors}");
            }

            return new ReturnDto<bool>(true, true, "Password changed successfully.");
        }

    }


}




