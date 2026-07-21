
using DocumentFormat.OpenXml.Spreadsheet;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.RoleDetail;
using Kader.Services.Implementations.User;
using Kader.Services.Implementations.UserRoles;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kader.Areas.Backend.Controllers

{ 
   // [Authorize]
    [Area("Backend")]
   
    public class CatogryController : Controller
    {
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        private ICatogryService _Catogry; private IKeys _keys;
        private IUserRoleService _UserRoleService;
        private IRoleDetailService _RoleDetailService; private IProductService _Product;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public CatogryController(IUserRoleService UserRoleService, IRoleDetailService RoleDetailService, IProductService ProductService, ICatogryService CatogryService, IAuthenticateService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService; _Product = ProductService;
            _Catogry = CatogryService; _keys = new Keys(); _UserRoleService = UserRoleService;
            _httpContextAccessor = httpContextAccessor; _RoleDetailService = RoleDetailService;
        }
        public async Task<IActionResult> Catogry(int? CatogryId)
        {
            var viewModel = new List<ApiCatogryDto>() ;
            var token = await _userService.GetTokenAsync();
            if(token != null)
            {
            var GetCatogry = await _Catogry.GetItemInfoAsync(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), token); 
            if (GetCatogry.isSuccess)
            {
                viewModel =   GetCatogry.Result ; 
            }
            else
            {
                viewModel =   null ;
            }
            }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddRolegomla(ApiCatogryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReturnDto<bool> Catogry;
                    model.CatTypeID = _keys.CatType_gomla();


                    model.name = model.name ;
                    model.CatIDaPI = model.id;
                        var roleid = _userService.AddRoles(_httpContextAccessor.HttpContext, model.name, model.CatTypeID,null ).Result;

                        if (roleid.isSuccess)
                        {
                            IList<RoleDetailDto> roles = new List<RoleDetailDto>();

                             
                            roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteShippingPrice(), RoleId = roleid.Result }); 
                              roles.Add(new RoleDetailDto { URL = _keys.URL_GetproductByCat(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_Edit_Product(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_Product_gomla_details(), RoleId = roleid.Result });
                            roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteProduct(), RoleId = roleid.Result });
                            roles.Add(new RoleDetailDto { URL = _keys.URL_EditShippingPrice(), RoleId = roleid.Result });
                            roles.Add(new RoleDetailDto { URL = _keys.URL_AddRolegomla(), RoleId = roleid.Result });
                            var val2 = await _RoleDetailService.SaveRoleDetail(_httpContextAccessor.HttpContext, roles);
                            if (val2.isSuccess)
                            {
                            return Json(new { success = true, message = "تم الحفظ بنجاح :)!" });

                        }
                            else
                            return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });

                    }
                    else
                        return Json(new { success = false, message = roleid.ErrorMsg });

                    //  model.RoleId = roleid.Result;
                }
                else
                    return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });







            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });

            }
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleqta3a(ApiCatogryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReturnDto<bool> Catogry;
                    model.CatTypeID = _keys.CatType_qta3a();


                    model.name = model.name;
                    model.CatIDaPI = model.id;
                    var roleid = _userService.AddRoles(_httpContextAccessor.HttpContext, model.name, model.CatTypeID,null ).Result;

                    if (roleid.isSuccess)
                    {
                        IList<RoleDetailDto> roles = new List<RoleDetailDto>();


                        roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteShippingPrice(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_product_qta3a(), RoleId = roleid.Result });
                         roles.Add(new RoleDetailDto { URL = _keys.URL_GetproductByCat(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_Edit_Product_qta3a(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_Product_qta3a_details(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteProduct(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_EditShippingPrice(), RoleId = roleid.Result });
                        roles.Add(new RoleDetailDto { URL = _keys.URL_AddRoleqta3a(), RoleId = roleid.Result });
                        var val2 = await _RoleDetailService.SaveRoleDetail(_httpContextAccessor.HttpContext, roles);
                        if (val2.isSuccess)
                        {
                            return Json(new { success = true, message = "تم الحفظ بنجاح :)!" });

                        }
                        else
                            return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });

                    }
                    else
                        return Json(new { success = false, message = roleid.ErrorMsg });

                    //  model.RoleId = roleid.Result;
                }
                else
                    return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });







            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });

            }
        }


        [HttpPost]
        public async Task<IActionResult> CatogryAsync(Catogry_backendDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReturnDto<int> Catogry;
                    model.NewItem.TypeId = _keys.CatType_qta3a();


                    //model.NewItem.CatogryName = model.NewItem.CatogryName + "-" + _keys.CatType_qta3a_text();
                    if (model.NewItem.CatogryId == 0)
                    {
                        //var roleid = _userService.AddRoles(_httpContextAccessor.HttpContext, model.NewItem.CatogryName).Result;

                        //if (roleid.isSuccess)
                        //{
                        //    IList<RoleDetailDto> roles = new List<RoleDetailDto>();

                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_Subcat_qta3a(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteShippingPrice(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_Subcat_qta3a(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_root_qta3a(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_deleteCat_qta3a(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_product_qta3a(), RoleId = roleid.Result });

                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_Product_qta3a_details(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteProduct(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_EditShippingPrice(), RoleId = roleid.Result });
                        //    roles.Add(new RoleDetailDto { URL = _keys.URL_child_qta3a(), RoleId = roleid.Result });
                        //    var val2 = await _RoleDetailService.SaveRoleDetail(_httpContextAccessor.HttpContext, roles);
                        //    if (val2.isSuccess)
                        //    {
                        //        TempData["SuccessMessage"] = "تم الحفظ بنجاح :)!";
                        //        ModelState.Clear();
                        //    }
                        //}
                        //model.NewItem.RoleId = roleid.Result;
                    }

                    Catogry = _Catogry.SaveCatogry(_httpContextAccessor.HttpContext, model.NewItem).Result;

                    if (Catogry.isSuccess)
                    {
                        TempData["SuccessMessage"] = "تم الحفظ بنجاح :)!";
                        ModelState.Clear();

                    }

                }


                TempData["ErrorMessage"] = "حدثت مشكلة اثناء الحفظ للمنتج!";
                return RedirectToAction("Catogry");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
