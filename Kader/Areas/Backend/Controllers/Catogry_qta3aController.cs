
using DocumentFormat.OpenXml.Spreadsheet;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
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
   
    public class Catogry_qta3aController : Controller
    {
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        private ICatogryService _Catogry; private IKeys _keys;
        private IUserRoleService _UserRoleService;
        private IRoleDetailService _RoleDetailService; private IProductService _Product;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public Catogry_qta3aController(IUserRoleService UserRoleService, IRoleDetailService RoleDetailService, IProductService ProductService, ICatogryService CatogryService, IAuthenticateService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService; _Product = ProductService;
            _Catogry = CatogryService; _keys = new Keys(); _UserRoleService = UserRoleService;
            _httpContextAccessor = httpContextAccessor; _RoleDetailService = RoleDetailService;
        }
        public IActionResult Catogry(int? CatogryId)
        {
            if (User.Identity.IsAuthenticated)
            {//gUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
            }
            var viewModel = new Catogry_backendDto();
            var GetCatogry = _Catogry.GetCatogry<CatogryDto>(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
            if (GetCatogry.isSuccess)
            { 
                viewModel = new Catogry_backendDto
            {
                ExistingItems = GetCatogry.Result,
                NewItem = new CatogryDto()
            };
                if (CatogryId.HasValue)
                {
                    // This is an edit operation
                    var product = _Catogry.GetSingleCatogry(_httpContextAccessor.HttpContext, CatogryId.Value).Result;
                    if (product.isSuccess)
                    {
                        viewModel.NewItem = product.Result;
                    }
                }
            }
            else
                viewModel = new Catogry_backendDto
                {
                    ExistingItems = null,
                    NewItem = new CatogryDto()
                };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CatogryAsync( Catogry_backendDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ReturnDto<int> Catogry;
                    model.NewItem.TypeId =_keys.CatType_qta3a();
                    

                        model.NewItem.CatogryName = model.NewItem.CatogryName ;
                   

                        Catogry =await _Catogry.SaveCatogry(_httpContextAccessor.HttpContext,model.NewItem);
                   
                    if (Catogry.isSuccess)
                    {
                        //if (model.NewItem.CatogryId == 0)
                        //{
                        //    var roleid = _userService.AddRoles(_httpContextAccessor.HttpContext, model.NewItem.CatogryName+" - قطاعي", _keys.CatType_qta3a(), Catogry.Result).Result;

                        //    if (roleid.isSuccess)
                        //    {
                        //        IList<RoleDetailDto> roles = new List<RoleDetailDto>();
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_CreateProduct_qta3a(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_Product_qt3(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_Product_qta3a_details(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteProduct(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_EditShippingPrice(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_DeleteShippingPrice(), RoleId = roleid.Result });


                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_NewOrders(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_OrderDetails(), RoleId = roleid.Result });
                        //        roles.Add(new RoleDetailDto { URL = _keys.URL_UnderCheck(), RoleId = roleid.Result });
                        //        var val2 = await _RoleDetailService.SaveRoleDetail(_httpContextAccessor.HttpContext, roles);
                        //        if (val2.isSuccess)
                        //        {
                        //            ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                        //            ModelState.Clear();
                        //        }
                        //    }
                        //    model.NewItem.RoleId = roleid.Result;
                        //}
                        ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                        ModelState.Clear();

                    }
                        
                    }


                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("Catogry");
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public IActionResult SubCatogry( )
        {
            var viewModel = new Catogry_backendDto();
            // model.NewItem.TypeId = _keys.CatType_qta3a();
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
            var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_qta3a() ).Result;
            if (GetCatogry.isSuccess)
            {
                ViewBag.CatId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
                viewModel = new Catogry_backendDto
            {
                ExistingItems = GetCatogry.Result,
                NewItem = new CatogryDto()
            };
            }
            else
                viewModel = new Catogry_backendDto
                {
                    ExistingItems = null,
                    NewItem = new CatogryDto()
                };
            return View(viewModel);
          
        }
        [HttpPost]
        public async Task<IActionResult> SubCatogry(CatogryDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.TypeId = _keys.CatType_qta3a();
                    ReturnDto<int> doLogin;
                    doLogin = await _Catogry.SaveCatogry(_httpContextAccessor.HttpContext,model);
                    if (doLogin.isSuccess)
                    {
                        var val = Newtonsoft.Json.JsonConvert.SerializeObject(doLogin.Result);
                      //  return View("تم الحفظ بنجاح");

                        //   return new JsonResult(doLogin.ErrorMsg);
                    }
                    else
                    {
                    //    return new JsonResult(doLogin.ErrorMsg);
                    }
                }
                return RedirectToAction("SubCatogry");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task<string> CheckProductsInSubCategories(List<int> categoryIds)
        {
            var message = "";
            var checkProductByCatID = await _Product.checkProductByCatID(_httpContextAccessor.HttpContext, categoryIds);
            if (checkProductByCatID.isSuccess)
            {
                message = "يوجد منتجات تابعة لهذا الصنف يرجي مسحها اولا";
                return message;
            }
            else
            {
                var DeleteAllSubCatogry = _Catogry.DeleteAllSubCatogry(_httpContextAccessor.HttpContext, categoryIds).Result;
                if (DeleteAllSubCatogry.isSuccess)
                {
                    message = "تم الحذف";
                }
                else
                    message = "يوجد مشكلة";
            }
            return message;
        }

        [HttpPost]
        public async Task<JsonResult> DeleteCatAsync(int id, string RoleId)
        {
            List<int> myList = new List<int> { id };
            var message = "";
            //if there is no subcatogry so check product for parent id
            var GetSubCat = await _Catogry.RecursiveGetllSubCategories(myList);
            if (GetSubCat.Count != 0)
            {
                message = await CheckProductsInSubCategories(GetSubCat);
                if (message == "تم الحذف")
                {

                    var checkProductByCatID = _UserRoleService.DeleteRoles(_httpContextAccessor.HttpContext, RoleId).Result;
                    if (checkProductByCatID.isSuccess)
                    {
                        message = "تم الحذف";
                    }
                }
                else
                    message = "يوجد مشكلة";
            }
            return Json(message);
        }
    }
}
