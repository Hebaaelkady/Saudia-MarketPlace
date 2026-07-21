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

 
    public class PageController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor; 
    private IPagesService _Pages; 
    private IKeys _keys; 
        public PageController(IUnitsService UnitsService, IPagesService PagesService, IHttpContextAccessor httpContextAccessor)
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
   
}

