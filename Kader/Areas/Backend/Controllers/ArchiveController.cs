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
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Spire.Xls;
using Kader.Infrastructure.Shared.Implementations;

[Area("Backend")]

public class ArchiveController : Controller
{
    private ICatogryService _Catogry; IProductService _Product;
    private IHttpContextAccessor _httpContextAccessor;
    private IUnitsService _Units; private IColorsService _Colors;
    private IKeys _keys;
    public ArchiveController(IUnitsService UnitsService, IProductService ProductService, ICatogryService CatogryService, IColorsService ColorsService, IHttpContextAccessor httpContextAccessor)
    {
        _Units = UnitsService; _Colors = ColorsService;
        _httpContextAccessor = httpContextAccessor;
        _Product = ProductService;
        _Catogry = CatogryService; _keys = new Keys();
    }
    public IActionResult Colors()
    {
        var GetColors = _Colors.GetDeletedColors().Result;
        if (GetColors.isSuccess)
        {
            return View(GetColors.Result);
        }
        return View();
    }

    public IActionResult Units()
    {
        var GetUnits = _Units.GetDeletedUnits().Result;
        if (GetUnits.isSuccess)
        {
            return View(GetUnits.Result);
        }
        return View();
    }



    public IActionResult Catogry_gomla()
    {
        try
        {


            var GetCatogry = _Catogry.GetDeletedCatogry(_httpContextAccessor.HttpContext, null, _keys.CatType_gomla()).Result;
            if (GetCatogry.isSuccess)
            {
                return View(GetCatogry.Result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        return View();
    }
    public IActionResult Catogry_qta3a()
    {
        var GetCatogry = _Catogry.GetDeletedCatogry(_httpContextAccessor.HttpContext, null, _keys.CatType_qta3a()).Result;
        if (GetCatogry.isSuccess)
        {
            return View(GetCatogry.Result);
        }
        return View();
    }
    public IActionResult Product_gomla()
    {
        var GetProduct = _Product.GetAllDeletedProduct(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
        if (GetProduct.isSuccess)
        {
            return View(GetProduct.Result);
        }
        return View();
    }
    public IActionResult Product_qta3a()
    {
        var GetProduct = _Product.GetAllDeletedProduct(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
        if (GetProduct.isSuccess)
        {
            return View(GetProduct.Result);
        }
        return View();
    }
    [HttpPost]
    public JsonResult RestoreDeleteUnits(int id)
    {
        var message = "";
        var DeleteColors = _Units.RestoreDeleteUnits(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الاستعادة";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
    [HttpPost]
    public JsonResult RestoreDeleteColors(int id)
    {
        var message = "";
        var DeleteColors = _Colors.RestoreDeleteColors(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الاستعادة";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
    [HttpPost]
    public JsonResult RestoreDeleteCatogry(int id)
    {
        var message = "";
        var DeleteColors = _Catogry.RestoreDeleteCatogry(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الاستعادة";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
    [HttpPost]
    public JsonResult RestoreDeleteProducts(int id)
    {
        var message = "";
        var DeleteColors = _Product.RestoreDeleteProducts(_httpContextAccessor.HttpContext, id).Result;
        if (DeleteColors.isSuccess)
        {
            message = "تم الاستعادة";
        }

        else
            message = "يوجد مشكلة";

        return Json(message);
    }
}

