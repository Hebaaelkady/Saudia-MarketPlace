 
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
 
using Kader.DTOs.Roles;
using Kader.DTOs.UsersStores;
namespace Kader.Services.Implementations.UserRoles
{
    public class UserRoleService : IUserRoleService
    {

        private IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ICatogryService _Catogry;
        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        public UserRoleService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ICatogryService CatogryService,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration, IJwtFactory jwtFactory)
        {
            _jwtFactory = jwtFactory;
            _unitOfWork = unitOfWork; _Catogry = CatogryService;
              _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration; _mapper = ObjectMapper.Mapper;
        }

        public async Task<ReturnDto<List<UsersStoresDto>>> GetAllShippingUsers(HttpContext context,string role)
        {
            try
            {
               
                // Ensure that the Shipping Role exists
                var shippingRole = await _roleManager.FindByIdAsync(role);
                if (shippingRole == null)
                {
                    return new ReturnDto<List<UsersStoresDto>>(false, null, "Shipping Role not found.");
                }

                // Get users that have the Shipping Role
                var shippingUsers = await _userManager.GetUsersInRoleAsync(shippingRole.Name);
                if (shippingUsers == null || !shippingUsers.Any())
                {
                    return new ReturnDto<List<UsersStoresDto>>(false, null, "No users found with Shipping Role.");
                }

                // Map users to DTO
                var mappedUsers = shippingUsers.Select(u => new UsersStoresDto
                {
                    UserId = u.Id,  // Include UserId
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    CountryCode=u.CountryCode
                }).ToList();

                return new ReturnDto<List<UsersStoresDto>>(true, mappedUsers, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching shipping users: {ex.Message}");
                return new ReturnDto<List<UsersStoresDto>>(false, null, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ReturnDto<bool>> DeleteRoles (HttpContext context, string roleName)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var role = await _roleManager.FindByIdAsync(roleName);
            if (role != null)
            {
                role.IsDelete = true;
                role.DeletedDate = DateTime.UtcNow.AddHours(3);
                role.DeletedBy = stringUserId;
                var updateResult = await _roleManager.UpdateAsync(role);

                if (updateResult.Succeeded)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
            }
            
                return new ReturnDto<bool>(false, false, "exist!");

        }
        //[HttpPost]
        //public async Task<ReturnDto<string>> DeleteUserRoles(HttpContext context, string roleName, string userid)
        //{
        //    var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        //    var user = await _userManager1.FindByIdAsync(userid);
        //    if (user != null)
        //    {
        //        // Check if the user has the role
        //        if (await _userManager1.IsInRoleAsync(user, roleName))
        //        {
        //            var userRole = await _ApplicationRole.FindByNameAsync(roleName);
        //            userRole.IsDelete = true;
        //            userRole.DeletedDate = DateTime.Now;
        //            userRole.DeletedBy = stringUserId;
        //            await _ApplicationRole.UpdateAsync(userRole);
        //            if (await _unitOfWork.CompleteAsync() > 0)

        //                return new ReturnDto<string>(true, userRole.Id, string.Empty);

        //        }
        //        return new ReturnDto<string>(false, "", "exist!");
        //    }

        //    else
        //        return new ReturnDto<string>(false, "", "exist!");
        //}
       
        public async Task<ReturnDto<bool>> DeleteUser(HttpContext context, string id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    role.IsDelete = true;
                    role.DeletedDate = DateTime.UtcNow.AddHours(3);
                    role.DeletedBy = stringUserId;
                   var updateResult = await _roleManager.UpdateAsync(role);
                    
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
        public async Task<ReturnDto<List<RolesDto>>> GetUserRoles(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ReturnDto<List<RolesDto>>(false, null, "المستخدم غير موجود.");
                }

                // Get role names assigned to the user
                var roleNames = await _userManager.GetRolesAsync(user);

                // Fetch RoleId for each role name from RoleManager
                var rolesDtoList = new List<RolesDto>();
                foreach (var roleName in roleNames)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        rolesDtoList.Add(new RolesDto
                        {
                            RoleId = role.Id,  // Role ID
                            Name = role.Name   // Role Name
                        });
                    }
                }

                return new ReturnDto<List<RolesDto>>(true, rolesDtoList, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
                return new ReturnDto<List<RolesDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> UpdateUserRolesAsync(string userId, List<string> roles)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ReturnDto<bool>(false, false, "المستخدم غير موجود.");
                }

                var existingRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRoles);

                if (roles != null && roles.Any())
                {
                    await _userManager.AddToRolesAsync(user, roles);
                }

                return new ReturnDto<bool>(true, true, "تم تحديث أدوار المستخدم بنجاح.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, "حدث خطأ أثناء تحديث أدوار المستخدم.");
            }
        }
        public async Task<ReturnDto<List<RolesDto>>> GetAllRoles()
        {
            try
            {
                var Roles = _roleManager.Roles.Where(k => k.IsDelete == false).ToList();
                var map = _mapper.Map<List<RolesDto>>(Roles);
                return new ReturnDto<List<RolesDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locs.System.Error: {ex.Message}");
                return new ReturnDto<List<RolesDto>>(false, null, ex.Message);
            }
        }
    }
}




