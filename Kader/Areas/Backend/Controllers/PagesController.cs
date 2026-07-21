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
using Kader.Services.Implementations.Pages;
using Kader.DTOs.Pages;

[Area("Backend")]

    public class PagesController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor; 
    private IPagesService _Pages; 
    private IKeys _keys; 
        public PagesController(IUnitsService UnitsService, IPagesService PagesService, IHttpContextAccessor httpContextAccessor)
        {
         _Pages = PagesService;
        _httpContextAccessor = httpContextAccessor;
         
    }
    public IActionResult Contactus(int? id)
    {

        if (id.HasValue)
        {
            var GetColors = _Pages.GetSinglePages(1).Result;
            if (GetColors.isSuccess)
            {
                var viewModel = GetColors.Result;
                return View(viewModel);
            }
        }
        return View();
    }
    [HttpPost] 
    public async Task<IActionResult> Contactus([FromForm] PagesDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "الرجاء تصحيح الأخطاء والمحاولة مرة أخرى.";
                return View(model); // Return the view with the model to show validation errors
            }
            model.PageId = 1;
            var saveResult = await _Pages.SavePages(_httpContextAccessor.HttpContext, model);

            if (saveResult.isSuccess && saveResult.Result)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                ModelState.Clear();
                return RedirectToAction("Contactus"); // Redirect to the Contactus page after saving
            }

            ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ للمنتج!";
            return RedirectToAction("Contactus"); // Redirect back with an error message
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Contactus: {ex.Message}");
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع، يرجى المحاولة لاحقًا.";
            return RedirectToAction("Contactus"); // Redirect back with a generic error message
        }
    }

    public IActionResult Terms_Conditions(int? id)
    {

        if (id.HasValue)
        {
            var GetColors = _Pages.GetSinglePages(2).Result;
            if (GetColors.isSuccess)
            {
                var viewModel = GetColors.Result;
                return View(viewModel);
            }
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Terms_Conditions([FromForm] PagesDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "الرجاء تصحيح الأخطاء والمحاولة مرة أخرى.";
                return View(model); // Return the view with the model to show validation errors
            }
            model.PageId = 2;
            var saveResult = await _Pages.SavePages(_httpContextAccessor.HttpContext, model);

            if (saveResult.isSuccess && saveResult.Result)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                ModelState.Clear();
                return RedirectToAction("Terms_Conditions"); // Redirect to the Contactus page after saving
            }

            ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ للمنتج!";
            return RedirectToAction("Terms_Conditions"); // Redirect back with an error message
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Terms_Conditions: {ex.Message}");
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع، يرجى المحاولة لاحقًا.";
            return RedirectToAction("Terms_Conditions"); // Redirect back with a generic error message
        }
    }
    public IActionResult privacy_policy(int? id)
    {

        if (id.HasValue)
        {
            var GetColors = _Pages.GetSinglePages(3).Result;
            if (GetColors.isSuccess)
            {
                var viewModel = GetColors.Result;
                return View(viewModel);
            }
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> privacy_policy([FromForm] PagesDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "الرجاء تصحيح الأخطاء والمحاولة مرة أخرى.";
                return View(model); // Return the view with the model to show validation errors
            }
            model.PageId = 3;
            var saveResult = await _Pages.SavePages(_httpContextAccessor.HttpContext, model);

            if (saveResult.isSuccess && saveResult.Result)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                ModelState.Clear();
                return RedirectToAction("privacy_policy"); // Redirect to the Contactus page after saving
            }

            ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ للمنتج!";
            return RedirectToAction("privacy_policy"); // Redirect back with an error message
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in privacy_policy: {ex.Message}");
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع، يرجى المحاولة لاحقًا.";
            return RedirectToAction("privacy_policy"); // Redirect back with a generic error message
        }
    }
    public IActionResult Exchange_return_policy(int? id)
    {

        if (id.HasValue)
        {
            var GetColors = _Pages.GetSinglePages(4).Result;
            if (GetColors.isSuccess)
            {
                var viewModel = GetColors.Result;
                return View(viewModel);
            }
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Exchange_return_policy([FromForm] PagesDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "الرجاء تصحيح الأخطاء والمحاولة مرة أخرى.";
                return View(model); // Return the view with the model to show validation errors
            }
            model.PageId = 4;
            var saveResult = await _Pages.SavePages(_httpContextAccessor.HttpContext, model);

            if (saveResult.isSuccess && saveResult.Result)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                ModelState.Clear();
                return RedirectToAction("Exchange_return_policy"); // Redirect to the Contactus page after saving
            }

            ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ للمنتج!";
            return RedirectToAction("Exchange_return_policy"); // Redirect back with an error message
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Exchange_return_policy: {ex.Message}");
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع، يرجى المحاولة لاحقًا.";
            return RedirectToAction("Exchange_return_policy"); // Redirect back with a generic error message
        }
    }
    public IActionResult AboutUS(int? id)
    {

        if (id.HasValue)
        {
            var GetColors = _Pages.GetSinglePages(5).Result;
            if (GetColors.isSuccess)
            {
                var viewModel = GetColors.Result;
                return View(viewModel);
            }
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AboutUS([FromForm] PagesDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "الرجاء تصحيح الأخطاء والمحاولة مرة أخرى.";
                return View(model); // Return the view with the model to show validation errors
            }
            model.PageId = 5;
            var saveResult = await _Pages.SavePages(_httpContextAccessor.HttpContext, model);

            if (saveResult.isSuccess && saveResult.Result)
            {
                ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                ModelState.Clear();
                return RedirectToAction("AboutUS"); // Redirect to the Contactus page after saving
            }

            ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ للمنتج!";
            return RedirectToAction("AboutUS"); // Redirect back with an error message
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AboutUS: {ex.Message}");
            ViewBag.ErrorMessage = "حدث خطأ غير متوقع، يرجى المحاولة لاحقًا.";
            return RedirectToAction("AboutUS"); // Redirect back with a generic error message
        }
    }

}

