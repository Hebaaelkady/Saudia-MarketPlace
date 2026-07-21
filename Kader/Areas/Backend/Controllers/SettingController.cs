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
using Kader.Services.Implementations.BannerImgs;
using Kader.DTOs.Stores;
using Kader.Services.Implementations.Stores;
using Kader.Services.Implementations.Ads;

[Area("Backend")]

    public class SettingController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor; private IBannerImgsService _BannerImgs;
    private IUnitsService _Units; private IColorsService _Colors; private IStoresService _Stores;
    private IAdsService _Ads;
    private IKeys _keys; 
        public SettingController(IUnitsService UnitsService, IStoresService StoresService, IAdsService AdsService, IBannerImgsService BannerImgsService, IColorsService ColorsService, IHttpContextAccessor httpContextAccessor)
        {
        _Units = UnitsService; _Colors = ColorsService; _Stores = StoresService; _Ads = AdsService;
        _httpContextAccessor = httpContextAccessor;
        _BannerImgs = BannerImgsService;
    }
    public IActionResult Colors(int? ColorId)
    {
      
        var viewModel = new Colors_backendDto();
        var GetColors = _Colors.GetColors().Result;
        if (GetColors.isSuccess)
        {
            viewModel = new Colors_backendDto
            {
                ExistingItems = GetColors.Result,
                NewItem = new ColorsDto()
            };
            if (ColorId.HasValue)
            {
                // This is an edit operation
                var product = _Colors.GetSingleColors( ColorId.Value).Result;
                if (product.isSuccess)
                {
                    viewModel.NewItem = product.Result;
                }
            }
        }
        else
            viewModel = new Colors_backendDto
            {
                ExistingItems = null,
                NewItem = new ColorsDto()
            };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> ColorsAsync(Colors_backendDto model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                ReturnDto<bool> Catogry; 
                
                Catogry = _Colors.SaveColors(_httpContextAccessor.HttpContext, model.NewItem).Result;
                if (Catogry.isSuccess)
                {
                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                    ModelState.Clear();
                }

            }
            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("Colors");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    [HttpPost]
    public JsonResult DeleteColors(int id)
    {
        var message = "";
        var DeleteColors = _Colors.DeleteColors(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الحذف";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
    public IActionResult Units(int? UnitId)
    {
       
        var viewModel = new Units_backendDto();
        var GetUnits = _Units.GetUnits().Result;
        if (GetUnits.isSuccess)
        {
            viewModel = new Units_backendDto
            {
                ExistingItems = GetUnits.Result,
                NewItem = new UnitsDto()
            };
            if (UnitId.HasValue)
            {
                // This is an edit operation
                var product = _Units.GetSingleUnits( UnitId.Value).Result;
                if (product.isSuccess)
                {
                    viewModel.NewItem = product.Result;
                }
            }
        }
        else
            viewModel = new Units_backendDto
            {
                ExistingItems = null,
                NewItem = new UnitsDto()
            };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> UnitsAsync(Units_backendDto model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                ReturnDto<bool> Catogry;  
              
                Catogry = _Units.SaveUnits(_httpContextAccessor.HttpContext, model.NewItem).Result;
                if (Catogry.isSuccess)
                {
                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                    ModelState.Clear();
                }

            }
            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("Units");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }


    public JsonResult DeleteUnits(int id)
    {
        var message = "";
        var DeleteUnits = _Units.DeleteUnits(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteUnits.isSuccess)
        {
            message = "تم الحذف";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }


    public IActionResult Stores(int? StoresId)
    {

        var viewModel = new Stores_backendDto();
        var GetStores = _Stores.GetStores().Result;
        if (GetStores.isSuccess)
        {
            viewModel = new Stores_backendDto
            {
                ExistingStores = GetStores.Result,
                NewStores = new StoresDto()
            };
            if (StoresId.HasValue)
            {
                // This is an edit operation
                var product = _Stores.GetSingleStores(StoresId.Value).Result;
                if (product.isSuccess)
                {
                    viewModel.NewStores = product.Result;
                }
            }
        }
        else
            viewModel = new Stores_backendDto
            {
                ExistingStores = null,
                NewStores = new StoresDto()
            };
        return View(viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> StoresAsync(Stores_backendDto model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                ReturnDto<bool> Catogry;

                Catogry = _Stores.SaveStores(_httpContextAccessor.HttpContext, model.NewStores).Result;
                if (Catogry.isSuccess)
                {
                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                    ModelState.Clear();
                }

            }
           ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("Stores");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }


    public JsonResult DeleteStores(int id)
    {
        var message = "";
        var DeleteStores = _Stores.DeleteStores(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteStores.isSuccess)
        {
            message = "تم الحذف";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
    public async Task<IActionResult> GetBannerImgs()
    {
        var GetUnits = await _BannerImgs.GetBannerImg();
        return View(GetUnits.Result);
    }

    [HttpPost]
    public async Task<IActionResult> GetBannerImgs(List<IFormFile> ProductImg)
    {
        try
        {
            ReturnDto<bool> ProductImgReturn;
            if (ProductImg.Count != 0)
            {
                ProductImgReturn = await _BannerImgs.AddFiles(_httpContextAccessor.HttpContext, ProductImg);
                if (ProductImgReturn.isSuccess)
                {
                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للصور!";
                }
            }
            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("GetBannerImgs");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }


    [HttpPost]
    public JsonResult DeleteBannerImg(int id)
    {
        var message = "";
        var DeleteColors = _BannerImgs.DeleteBannerImg(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الحذف";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }




    public async Task<IActionResult> Ads(int? ColorId)
    {
        var GetUnits = await _Ads.GetAds();
        return View(GetUnits.Result);
    }

    [HttpPost]
    public async Task<IActionResult> AdsAsync(List<IFormFile> ProductImg, List<IFormFile> ProductImg1)
    {
        try
        {
            ReturnDto<bool> ProductImgReturn;
            if (ProductImg.Count != 0|| ProductImg1.Count != 0)
            {
                ProductImgReturn = await _Ads.AddFiles(_httpContextAccessor.HttpContext, ProductImg, ProductImg1);
                if (ProductImgReturn.isSuccess)
                {
                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للصور!";
                }
            }
            
            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
            return RedirectToAction("Ads");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }


    [HttpPost]
    public JsonResult DeleteAds(int id)
    {
        var message = "";
        var DeleteColors = _Ads.DeleteAds(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الحذف";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
}

