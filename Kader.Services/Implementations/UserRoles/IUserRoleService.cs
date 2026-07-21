 
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
using Kader.DTOs.Roles;
using Kader.DTOs.UsersStores;

namespace Kader.Services.Implementations.UserRoles
{
    public interface IUserRoleService
    {
        Task<ReturnDto<List<UsersStoresDto>>> GetAllShippingUsers(HttpContext context, string role);
        Task<ReturnDto<bool>> DeleteRoles(HttpContext context, string roleName);
        Task<ReturnDto<List<RolesDto>>> GetAllRoles();
        Task<ReturnDto<bool>> DeleteUser(HttpContext context, string id);
        Task<ReturnDto<List<RolesDto>>> GetUserRoles(string id);
        Task<ReturnDto<bool>> UpdateUserRolesAsync(string userId, List<string> roles);
    }
}




