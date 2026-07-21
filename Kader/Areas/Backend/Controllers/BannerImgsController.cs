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
using Kader.DTOs.BannerImgs;

[Area("Backend")]

    public class BannerImgsController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
    private IUnitsService _Units; private IBannerImgsService _BannerImgs;
    private IKeys _keys; 
        public BannerImgsController(IUnitsService UnitsService, IBannerImgsService BannerImgsService, IHttpContextAccessor httpContextAccessor)
        {
        _Units = UnitsService; _BannerImgs = BannerImgsService;
        _httpContextAccessor = httpContextAccessor;

        }
   public async Task<IActionResult> GetBannerImgs()
{
    var GetUnits = await _BannerImgs.GetBannerImg();
    return View(GetUnits);
}

    [HttpPost]
    public async Task<IActionResult> GetBannerImgs(List<IFormFile> ProductImg)
    {
        try
        {
            ReturnDto<bool> ProductImgReturn;
            if (ProductImg.Count != 0)
            {
                ProductImgReturn =await _BannerImgs.AddFiles(_httpContextAccessor.HttpContext, ProductImg);
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
            return RedirectToAction("BannerImgs");
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
    

}

