
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.Users;
using Kader.Infrastructure.Jwt;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Implementations.Ads;
using Kader.Services.Implementations.BannerImgs;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.ProductImg;
using Kader.Services.Implementations.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kader.Controllers
{

    public class HomeController : Controller
    {
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        private IAdsService _Ads;
        private ICatogryService _Catogry; private IProductImgService _ProductImg;
        private IProductService _Product; private IBannerImgsService _BannerImgs; private IKeys _keys;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public HomeController(IAuthenticateService userService, ICatogryService CatogryService, IAdsService AdsService, IBannerImgsService BannerImgsService, IHttpContextAccessor httpContextAccessor, IProductImgService ProductImgService, IProductService ProductService)
        {
            _userService = userService;
            _Catogry = CatogryService; _Product = ProductService; _BannerImgs = BannerImgsService;
            _Ads = AdsService;
            _ProductImg = ProductImgService;
            _httpContextAccessor = httpContextAccessor; _keys = new Keys();
        }

        
         


        public async Task<IActionResult> Index()
        {
            var model=new HomeDto();
           
                var GetspecialProduct = await _Product.SpecialOrder(_keys.CatType_gomla(),  10);
                if (GetspecialProduct.isSuccess)
                {

                    model.specialProductGomla = GetspecialProduct.Result;
                }
            var GetspecialProductQt3 = await _Product.SpecialOrder(_keys.CatType_qta3a(), 10);
            if (GetspecialProductQt3.isSuccess)
            {

                model.specialProductQt3 = GetspecialProductQt3.Result;
            }

            var GoodPriceGomla = await _Product.GoodPrice(_keys.CatType_gomla(),  10);
            if (GoodPriceGomla.isSuccess)
            {

                model.GoodPriceGomla = GoodPriceGomla.Result;
            }
            var GoodPriceQt3 = await _Product.GoodPrice(_keys.CatType_qta3a(), 10);
            if (GoodPriceQt3.isSuccess)
            {

                model.GoodPriceQt3 = GoodPriceQt3.Result;
            }
            var BannerImgs  = _BannerImgs.GetBannerImg().Result;
            if (BannerImgs.isSuccess)
            {
                model.BannerImgsDto = BannerImgs.Result;
            }

            var AdsDto = _Ads.GetLatestAds().Result;
            if (AdsDto.isSuccess)
            {
                model.AdsDto =  AdsDto.Result;
            }
            return View(model);
         //   return View();
        }
        public async Task<IActionResult> details(int? id)
        {
            var viewModel = new ApiEditProductDto();

            if (id.HasValue)
            {
                var token = await _userService.GetTokenAsync();
                if (token != null)
                {
                    var product = await _Product.GetSingleProduct(_httpContextAccessor.HttpContext, id.Value);
                    if (product.isSuccess)
                    {
                        if (product.Result != null)
                        {  
                        var GetAllProductID = product.Result.ApiProdID;
                        var GetProductByProductApi = await _Product.GetProductByIDApi(token, product.Result.ApiProdID); // Pass Type directly
                        if (GetProductByProductApi.isSuccess)
                        {
                            viewModel=new ApiEditProductDto
                                {
                                title = GetProductByProductApi.Result.title, // Get title from apiProduct
                                ApiProdID = GetProductByProductApi.Result.p_id,
                                p_id = GetProductByProductApi.Result.p_id,
                                AfterDiscount = product.Result.AfterDiscount,
                                price = GetProductByProductApi.Result.price,
                                ProductId = product.Result.ProductId,
                                stock = GetProductByProductApi.Result.stock
                                ,
                                CatTypeId = product.Result.CatTypeId.Value,
                                ProductImgDto = product.Result.ProductImgDto
                                };
                            }
                        }
                        return View(viewModel);
                    }
                }
            }
            return View(null);

        }
    

        public async Task<JsonResult> GetRoot(string id)
        {
            List<ApiCatogryDto> categoryResult = new List<ApiCatogryDto>();
           
                var categoryResult1 = await _Catogry.GetCatogry<ApiCatogryDto>(_httpContextAccessor.HttpContext, _keys.CatType_gomla());
                if (categoryResult1.isSuccess)
                {
                    categoryResult = categoryResult1.Result ?? new List<ApiCatogryDto>();
                }
             
            return Json(categoryResult);
        }

        public async Task<JsonResult> GetRootQt2a(string id)
        {
            List<ApiCatogryDto> categoryResult = new List<ApiCatogryDto>();
            
                var categoryResult1 = await _Catogry.GetCatogry<ApiCatogryDto>(_httpContextAccessor.HttpContext, _keys.CatType_qta3a());
                if (categoryResult1.isSuccess)
                {
                    categoryResult = categoryResult1.Result ?? new List<ApiCatogryDto>();
                }
             
            return Json(categoryResult);
        }

    }
}
