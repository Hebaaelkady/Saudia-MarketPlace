 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kader.DTOs.Users;
using Kader.DTOs;
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Roles;

namespace Kader.Services.Implementations.User
{
    public interface IAuthenticateService
    {
        Task<ReturnDto<LoginUserDto>> Login(string userName, string password, int TypeUser, HttpContext context);
         Task<ReturnDto<string>> Register(HttpContext context, AllUserViewDto model);
        Task<ReturnDto<AllUserViewDto>> GetUserID(string id);
        Task<ReturnDto<string>> AddRoles(HttpContext context, string roleName, int? CatTypeID, int? CatIDaPI);
        Task<ReturnDto<bool>> AddUserToRoles(HttpContext context, string userid, IList<string> roleName); 
        Task<ReturnDto<List<AllUserViewDto>>> GetAllUsers();
        Task<ReturnDto<bool>> DeleteUser(HttpContext context, string id);
        Task<ReturnDto<AllUserViewDto>> GetUser(HttpContext context);
        Task<string> GetTokenAsync();
        Task<ReturnDto<bool>> UpdateUserDetailsAsync(AllUserViewDto model);
        Task<ReturnDto<int>> SaveCustomerApiDtoApi(string token, CustomerApiDto order);
        ReturnDto<bool> checkExist(  string username);
        Task<ReturnDto<bool>> ChangePasswordAsync(HttpContext context,ChangePasswordDto model);
        Task<(ApplicationUser user, string CountryCode)> FindByMobileNumberAsync(string mobileNumber);
        string GenerateOtpForMobile(string mobileNumber);
        bool ValidateOtp(string mobileNumber, string otp);
          Task<ReturnDto<bool>> ResetPasswordAsync(string mobileNumber, string newPassword);
        Task<ReturnDto<List<GetCatogryByRoleDto>>> GetCatogryByRole(HttpContext context, int? type);
        Task<ReturnDto<List<UserPhoneDto>>> GetUsersFront();
        Task StoreOtpAsync(string mobileNumber, int otpId);
        Task<int?> GetStoredOtpIdAsync(string mobileNumber);
        Task<ReturnDto<bool>> UpdatePasswordAsync(string mobileNumber, string mobile, string newPassword);

    }
}




