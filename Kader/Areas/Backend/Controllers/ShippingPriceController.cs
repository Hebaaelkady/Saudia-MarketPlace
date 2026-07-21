using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.ShippingPrice;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.CatType;
using Kader.Services.Implementations.Colors;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.ProductImg;
using Kader.Services.Implementations.ShippingPrice;
using Kader.Services.Implementations.Units;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spire.Xls;
using System.Collections.Generic;

namespace Kader.Areas.Backend.Controllers
{   
    [Area("Backend")]

    public class ShippingPriceController : Controller
    {
     
   
            private IHttpContextAccessor _httpContextAccessor;
            private IShippingPriceService _ShippingPrice;
            private IKeys _keys;
            public List<CatogryDto> Gov_codesDetail { get; set; }
            public ShippingPriceController(IShippingPriceService ShippingPriceService, IHttpContextAccessor httpContextAccessor)
            {
                _ShippingPrice = ShippingPriceService;
                _httpContextAccessor = httpContextAccessor;

            }
            public ActionResult Edit()
            {
               
                return View();
            }
            // GET: ShippingPriceController/Edit/5
            public ActionResult Edit(int id)
            {
                var GetProduct = _ShippingPrice.GetShippingPrice(_httpContextAccessor.HttpContext, id).Result;
                var viewModel = new List<ShippingPriceDto>();
                if (GetProduct.isSuccess)
                {
                    viewModel = GetProduct.Result;
                }
                return View(viewModel);
            }

            // POST: ShippingPriceController/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(int id, IFormCollection collection)
            {
                try
                {
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            // GET: ShippingPriceController/Delete/5
            public ActionResult Delete(int id)
            {
                return View();
            }

            // POST: ShippingPriceController/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Delete(int id, IFormCollection collection)
            {
                try
                {
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
        }
     
}

