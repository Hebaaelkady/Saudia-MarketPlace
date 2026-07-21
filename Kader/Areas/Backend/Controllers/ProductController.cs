
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.Users;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.CatType;
using Kader.Services.Implementations.Colors;
using Kader.Services.Implementations.Units;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kader.Infrastructure.Shared;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Services.Implementations.ProductImg;
using Kader.Services.Implementations.ShippingPrice;
using System.Linq;
using Kader.DTOs.ShippingPrice;
using System.Net;
using DocumentFormat.OpenXml.Office2010.Excel;
using Kader.DTOs.ProductImg;
using Kader.Services.Implementations.LogPrice;
using Kader.DTOs.LogPrice;
using Kader.Services.Implementations.User;
using Kader.Infrastructure.Jwt;
using Kader.Services.Implementations.LogQuantity;
using Kader.DTOs.LogQuantity;
namespace Kader.Areas.Backend.Controllers

{
    [Area("Backend")]

    public class ProductController : Controller
    {
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        private ICatogryService _Catogry; private ILogQuantityService _LogQuantity;
        private IProductService _Product; private IProductImgService _ProductImg;
        private ICatTypeService _CatType; private ILogPriceService _LogPrice;
        private IUnitsService _Units; private IColorsService _Colors; private IShippingPriceService _ShippingPrice;
        private IKeys _keys;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public ProductController(ILogQuantityService LogQuantityService, IAuthenticateService userService, ILogPriceService LogPriceService, IColorsService ColorsService, IShippingPriceService ShippingPriceService, IProductImgService ProductImgService, IProductService ProductService, IUnitsService UnitsService, ICatTypeService CatTypeService, ICatogryService CatogryService, IHttpContextAccessor httpContextAccessor)
        {
            _LogQuantity = LogQuantityService; _userService = userService;
            _CatType = CatTypeService; _LogPrice = LogPriceService; _ProductImg = ProductImgService;
            _Catogry = CatogryService; _Product = ProductService; _ShippingPrice = ShippingPriceService;
            _httpContextAccessor = httpContextAccessor;
            _Units = UnitsService; _keys = new Keys();
            _Colors = ColorsService;
        }
        public async Task<IActionResult> Product_qta3a_Added(int? ProductId)
        {
            //var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;

            var viewModel = new List<ApiEditProductDto>();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetCatogry = await _Catogry.GetItemInfoAsync(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), token);
                if (GetCatogry.isSuccess)
                {
                    ViewBag.CatId = new SelectList(GetCatogry.Result, "id", "name");
                }
                var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), null);

                if (GetAllProduct.isSuccess)
                {


                    //};
                    var GetAllProductID = GetAllProduct.Result.Select(l => l.ApiProdID).OrderBy(id => id).ToList();
                    var GetProductByListProductApi = await _Product.GetProductByListProductApi(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), token, GetAllProductID, 0); // Pass Type directly


                    if (GetProductByListProductApi.isSuccess)
                    {
                        foreach (var localProduct in GetProductByListProductApi.Result)
                        {
                            var apiProduct = GetAllProduct.Result
                                .FirstOrDefault(p => p.ApiProdID == localProduct.p_id);

                            if (apiProduct != null)
                            {
                                viewModel.Add(new ApiEditProductDto
                                {
                                    Description = apiProduct.Description, // Get description from localProduct
                                    title = localProduct.title, // Get title from apiProduct
                                                                //barcode = apiProduct.barcode, // Get barcode from apiProduct
                                    ApiProdID = apiProduct.p_id,
                                    p_id = localProduct.p_id,
                                    //                     title = item.title,
                                    //                     price = item.price,
                                    //                     barcode = item.barcode,
                                    //                     stock = item.stock,
                                    ProductId = apiProduct.ProductId

                                });
                            }
                        }
                        return View(viewModel);
                        //viewModel = GetProductByListProductApi.Result;
                        //  return Json(new { success = true, data = viewModel });

                    }
                }


            }
            return View(viewModel);
        }
        public async Task<ActionResult> Product_qta3a_details1(int? p_id, int? ProductId)
        {
            var viewModel = new ApiEditProductDto();

            if (ProductId.HasValue)
            {
                // This is an edit operation
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
                var token = await _userService.GetTokenAsync();
                if (token != null)
                {
                    var GetProduct = await _Product.GetProductByIDApi(token, p_id.Value);
                    if (GetProduct.isSuccess)
                    {
                        viewModel = GetProduct.Result;

                        var product = _Product.GetSingleProducts(_httpContextAccessor.HttpContext, ProductId.Value, GetCatogryByRole.Result).Result;
                        if (product.isSuccess)
                        {
                            if (product.Result != null)
                            {
                                viewModel = product.Result;

                                viewModel.title = GetProduct.Result.title;
                                viewModel.barcode = GetProduct.Result.barcode;
                                //viewModel.barcode = GetProduct.Result.barcode;
                                viewModel.stock = GetProduct.Result.stock;
                                viewModel.price = GetProduct.Result.price;
                                viewModel.unites1 = GetProduct.Result.unites.FirstOrDefault().unitName;

                            }
                        }
                    }
                    else
                        viewModel = null;
                }
            }
            return View(viewModel);
            //  return RedirectToAction(nameof(HomesController.UnAuthorized), "Homes");

        }

        public IActionResult Product_gomla_details(int? id,int? mark)
        {
            var viewModel = new Product_backendDto();

            if (id.HasValue)
            {
                // This is an edit operation
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
                 
                var product = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, id.Value ).Result;
                if (product.isSuccess)
                {
                    if (product.Result != null)
                    {
                        viewModel = new Product_backendDto
                        {
                            NewItem = product.Result,

                        };
                        return View(viewModel);
                    }


                }
            }
            return RedirectToAction(nameof(HomesController.UnAuthorized), "Homes");

        }
        public IActionResult Product_qta3a_details(int? id)
        {
            var viewModel = new Product_backendDto();

            if (id.HasValue)
            {
                // This is an edit operation
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
                var product = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, id.Value ).Result;
                if (product.isSuccess)
                {
                    if (product.Result != null)
                    {
                        viewModel = new Product_backendDto
                        {
                            NewItem = product.Result,

                        };
                        return View(viewModel);
                    }


                }
            }
            return RedirectToAction(nameof(HomesController.UnAuthorized), "Homes");

        }


         public async Task<IActionResult> GetproductByCatApi(int Type)
        {
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;

            var viewModel = new List<ApiEditProductDto>();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetCatogry = await _Product.GetProductByCatApi(token, Type); // Pass Type directly
                if (GetCatogry.isSuccess)
                {
                    return Json(new { success = true, data = GetCatogry.Result }); // Return as JSON
                }
                 
            }
            else
            {
                return Json(new { success = false, message = "Failed to retrieve token" });
            }
            return Json(new { success = false, message = "" });
        }
        public async Task<IActionResult> GetproductByCat(int Type)
        {
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;

            var viewModel = new List<ApiEditProductDto>();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext,_keys.CatType_gomla(), null);
            
                if (GetAllProduct.isSuccess)
                {
                    

                    //};
                    var GetAllProductID = GetAllProduct.Result.Select(l => l.ApiProdID).ToList();
                        var GetProductByListProductApi = await _Product.GetProductByListProductApi(_httpContextAccessor.HttpContext, _keys.CatType_gomla(),token, GetAllProductID, Type); // Pass Type directly
                   
                    
                    if (GetProductByListProductApi.isSuccess)
                    {
                        foreach (var localProduct in GetAllProduct.Result)
                        {
                            var apiProduct = GetProductByListProductApi.Result
                                .FirstOrDefault(p => p.p_id == localProduct.ApiProdID);

                            if (apiProduct != null)
                            {
                                viewModel.Add(new ApiEditProductDto
                                {
                                    Description = localProduct.Description, // Get description from localProduct
                                    title = apiProduct.title, // Get title from apiProduct
                                    //barcode = apiProduct.barcode, // Get barcode from apiProduct
                                ApiProdID = localProduct.p_id,
                                                        p_id = apiProduct.p_id,
                                                          ProductId = localProduct.ProductId,
                                    //                     price = item.price,
                                    //                     barcode = item.barcode,
                                    //                     stock = item.stock,
                                    //                     ProductId= GetSingleProducts.Result.ProductId

                                });
                            }
                        }

                        //viewModel = GetProductByListProductApi.Result;
                        return Json(new { success = true, data = viewModel });

                    }                   
                }

                return Json(new { success = false, message = "Failed " });
            }
            else
            {
                return Json(new { success = false, message = "Failed to retrieve token" });
            }
        }

        public async Task<IActionResult> GetproductByCat_qta3a(int Type)
        {
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;

            var viewModel = new List<ApiEditProductDto>();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), null);

                if (GetAllProduct.isSuccess)
                {


                    //};
                    var GetAllProductID = GetAllProduct.Result.Select(l => l.ApiProdID).ToList();
                    var GetProductByListProductApi = await _Product.GetProductByListProductApi(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), token, GetAllProductID, Type); // Pass Type directly


                    if (GetProductByListProductApi.isSuccess)
                    {
                        foreach (var localProduct in GetAllProduct.Result)
                        {
                            var apiProduct = GetProductByListProductApi.Result
                                .FirstOrDefault(p => p.p_id == localProduct.ApiProdID);

                            if (apiProduct != null)
                            {
                                viewModel.Add(new ApiEditProductDto
                                {
                                    Description = localProduct.Description, // Get description from localProduct
                                    title = apiProduct.title, // Get title from apiProduct
                                                              //barcode = apiProduct.barcode, // Get barcode from apiProduct
                                    ApiProdID = localProduct.p_id,
                                    p_id = apiProduct.p_id,
                                    ProductId = localProduct.ProductId,
                                    //                     price = item.price,
                                    //                     barcode = item.barcode,
                                    //                     stock = item.stock,
                                    //                     ProductId= GetSingleProducts.Result.ProductId

                                });
                            }
                        }

                        //viewModel = GetProductByListProductApi.Result;
                        return Json(new { success = true, data = viewModel });

                    }
                }

                return Json(new { success = false, message = "Failed " });
            }
            else
            {
                return Json(new { success = false, message = "Failed to retrieve token" });
            }
        }
        public IActionResult Product_gomla(int? ProductId)
        {
            var viewModel = new Product_backendDto();
            //var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
            var GetProduct = _Product.GetAllProduct(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), null).Result;
            if (GetProduct.isSuccess)
            {
                viewModel = new Product_backendDto
                {
                    ExistingItems = GetProduct.Result,
                    NewItem = new ProductDto()
                };
            }
            else
                viewModel = new Product_backendDto
                {
                    ExistingItems = null,
                    NewItem = new ProductDto()
                };
            //   var product = null;

            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }
            var GetUnits = _Units.GetUnits().Result;
            if (GetUnits.isSuccess)
            {
                ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
            }
            var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
            if (GetCatogry.isSuccess)
            {
                ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
            }
            ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };
         
            return View(viewModel);
        }
        public IActionResult CreateProduct_gomla(int? ProductId)
        {
            var viewModel = new Product_backendDto();
              

            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }
            var GetUnits = _Units.GetUnits().Result;
            if (GetUnits.isSuccess)
            {
                ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
            }
            var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
            if (GetCatogry.isSuccess)
            {
                ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
            }
            //ViewBag.TransportMethodId = new List<SelectListItem>
            //    {
            //       // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
            //        new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
            //        new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
            //    };
            if (ProductId.HasValue)
            {
                // This is an edit operation 
                var product = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, ProductId.Value ).Result;
                if (product.isSuccess)
                {
                    if (product.Result != null)
                    {
                        viewModel.NewItem = product.Result;
                        if (GetColors.isSuccess)
                        {
                            ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName", product.Result.ColorId);
                        }
                        ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName",product.Result.CatogryId);

                        if (GetUnits.isSuccess)
                        {
                            ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName", product.Result.Unit);
                        }
                        ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString(), Selected = _keys.TransportMethodByme() == product.Result.TransportMethodId },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString(), Selected = _keys.TransportMethodByCompany() ==  product.Result.TransportMethodId  }
                };
                    }
                    else
                        return RedirectToAction(nameof(HomesController.UnAuthorized), "Homes");

                }
            }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct_gomla(ProductDto model, List<IFormFile> ProductImg)
        {
            try
            {
                var viewModel = new Product_backendDto();
                var CheckExistByCode = await _Product.CheckExistByCode(_httpContextAccessor.HttpContext, model.barcode,_keys.CatType_gomla());
                 
                var GetProduct =await _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId );
                if (GetProduct.isSuccess)
                {
                    
                    viewModel = new Product_backendDto
                    {
                        ExistingItems = null,
                        NewItem = GetProduct.Result
                    };
                }
                if (model.ApearInHomePage != true)
                {
                    model.ApearInHomePage = false;
                }
                else
                    model.ApearInHomePage = true;
                if (model.SpecialOrder != true)
                {
                    model.SpecialOrder = false;
                }
                else
                    model.SpecialOrder = true;
                if (!ModelState.IsValid || (CheckExistByCode.isSuccess && model.ProductId == 0) || (model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null)))

                {
                    var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_gomla() ).Result;
                    if (GetCatogry.isSuccess)
                    {
                        ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
                    }
                    var GetColors = _Colors.GetColors().Result;
                    if (GetColors.isSuccess)
                    {
                        ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
                    }
                    var GetUnits = _Units.GetUnits().Result;
                    if (GetUnits.isSuccess)
                    {
                        ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
                    }
                    if (CheckExistByCode.isSuccess && model.ProductId==0)
                    {
                        ViewBag.check = "الكود مكرر "; 
                    }
                    ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };

                    if (model.CatogryId == null)
                    {
                        ViewBag.CatogryIdMSG = "من فضلك اختار الصنف";
                    }
                    if (model.SubCatogryTitle == null)
                    {
                        ViewBag.SubCatogryTitle = "من فضلك ادخل العنوان الرئيسي";
                    }
                    if (model.barcode == null)
                    {
                        ViewBag.barcode = "من فضلك ادخل الكود";
                    }
                    if (model.BeforeDiscount == null)
                    {
                        ViewBag.Price = "من فضلك ادخل السعر";
                    }
                    if (model.BeforeDiscount < model.AfterDiscount&& model.AfterDiscount!=null)
                    {
                        ViewBag.AfterDiscount = "يجب ان يكون السعر اقل";
                    }
                    if (model.Unit == null)
                    {
                        ViewBag.Units = "من فضلك ادخل الوحدة";
                    }
                    if (model.QuantityAvailable == null)
                    {
                        ViewBag.QuantityAvailable = "من فضلك ادخل الكمية المتاحة";
                    }
                    if (model.MinQuantityToShipJomla == null)
                    {
                        ViewBag.MinQuantityToShipJomla = "من فضلك ادخل اقل كمية يمكن للمستخدم شراؤها";
                    }
                    
                    if (model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                    {
                        ViewBag.ShippingPrice = "من فضلك ادخل سعر الشحن";
                    }
                    return View(viewModel);
                }
                else
                {
                    model.CatTypeId = _keys.CatType_gomla();
                    if (model.ProductId != 0)
                    {
                        //var CheckPrices = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId, GetCatogryByRole.Result).Result;

                        //if (CheckPrices.isSuccess)
                        //{
                            if (!GetProduct.Result.AfterDiscount.Equals(model.AfterDiscount) || !GetProduct.Result.BeforeDiscount.Equals(model.BeforeDiscount) || !GetProduct.Result.discountBeginDate.Equals(model.discountBeginDate) || !GetProduct.Result.DiscountEndDate.Equals(model.DiscountEndDate))
                            {
                                var LogPriceDto = new LogPriceDto();
                                LogPriceDto.discountBeginDate = GetProduct.Result.discountBeginDate;

                                LogPriceDto.discountEndDat = GetProduct.Result.DiscountEndDate;
                                LogPriceDto.AfterDiscount = GetProduct.Result.AfterDiscount;
                                LogPriceDto.BeforeDiscount = GetProduct.Result.BeforeDiscount;

                                LogPriceDto.ProductId = GetProduct.Result.ProductId;
                                var LogPrices = _LogPrice.SaveLogPrice(_httpContextAccessor.HttpContext, LogPriceDto).Result;
                                if (LogPrices.isSuccess)
                                {

                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                                }
                            }
                            if (model.QuantityAdded!=0)
                            { 
                            var LogQuantityDto = new LogQuantityDto();
                            LogQuantityDto.NewQuantity = model.QuantityAdded;

                            LogQuantityDto.RemainingQuantity = GetProduct.Result.QuantityAvailable;
                            LogQuantityDto.ProductId = GetProduct.Result.ProductId;
                            var LogQuantity = _LogQuantity.SaveLogQuantity(_httpContextAccessor.HttpContext, LogQuantityDto).Result;
                                if (LogQuantity.isSuccess)
                                {

                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                                }
                            }
                        ////}

                    }
                    
                    if (model.ProductId != 0)
                    {
                        model.QuantityAvailable= model.QuantityAdded + GetProduct.Result.QuantityAvailable;
                    }
                    int ProductID = _Product.SaveProductInsideSite(_httpContextAccessor.HttpContext, model).Result;
                    ReturnDto<bool> ProductImgReturn;
                    if (ProductImg.Count != 0)
                    {
                        ProductImgReturn = _ProductImg.AddFiles(_httpContextAccessor.HttpContext, ProductImg, ProductID).Result;
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
                    else
                    {
                        ViewBag.SuccessMessage = "";
                        // ModelState.Clear();
                    }
                    if (model.ProductId == 0)
                    {
                        var LogQuantityDto = new LogQuantityDto();
                        LogQuantityDto.NewQuantity = model.QuantityAvailable;

                        LogQuantityDto.RemainingQuantity = 0;
                        LogQuantityDto.ProductId = ProductID;
                        var LogQuantity = _LogQuantity.SaveLogQuantity(_httpContextAccessor.HttpContext, LogQuantityDto).Result;
                        if (LogQuantity.isSuccess)
                        {

                        }
                        else
                        {
                            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                        }

                        
                        
                        if (model.ShippingPrice == null && model.ShippingPriceDto.Count != 0)
                        {
                            //عشان بيبعت null with row
                            var f = model.ShippingPriceDto.FirstOrDefault().VarPrice;
                            if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null) && model.ShippingPriceDto.Count() == 1)
                            {
                                if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                            }
                            else
                            {


                                ReturnDto<bool> ShippingPriceReturn = await _ShippingPrice.SaveShippingPrice(_httpContextAccessor.HttpContext, model.ShippingPriceDto, false, ProductID);
                                if (ShippingPriceReturn.isSuccess)
                                {
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                                    ModelState.Clear();
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للسعر المتغير!";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "";
                            // ModelState.Clear();
                        }
                    }
                    else
                        ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                    if (model.ProductId != 0)
                    {
                        return RedirectToAction("Product_gomla", new { ProductId = ProductID });
                    }
                    else
                        return RedirectToAction("Product_gomla");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ !";
                return null;
            }
        }
        public IActionResult Product_qta3a(int? ProductId)
        {
            var viewModel = new Product_backendDto();
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
            var GetProduct = _Product.GetAllProduct(_httpContextAccessor.HttpContext, _keys.CatType_qta3a(), GetCatogryByRole.Result).Result;
            if (GetProduct.isSuccess)
            {
                viewModel = new Product_backendDto
                {
                    ExistingItems = GetProduct.Result,
                    NewItem = new ProductDto()
                };
            }
            else
                viewModel = new Product_backendDto
                {
                    ExistingItems = null,
                    NewItem = new ProductDto()
                };
            //   var product = null;

            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }
            var GetUnits = _Units.GetUnits().Result;
            if (GetUnits.isSuccess)
            {
                ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
            }
            var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_qta3a() ).Result;
            if (GetCatogry.isSuccess)
            {
                ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
            }
            ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };
      

            return View(viewModel);
        }
        public IActionResult CreateProduct_qta3a(int? ProductId)
        {
            var viewModel = new Product_backendDto();
             

            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }
            var GetUnits = _Units.GetUnits().Result;
            if (GetUnits.isSuccess)
            {
                ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
            }
            var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
            if (GetCatogry.isSuccess)
            {
                ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
            }
            //ViewBag.TransportMethodId = new List<SelectListItem>
            //    {
            //       // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
            //        new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
            //        new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
            //    };
            if (ProductId.HasValue)
            {
                // This is an edit operation 
                var product = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, ProductId.Value ).Result;
                if (product.isSuccess)
                {
                    if (product.Result != null)
                    {
                        viewModel.NewItem = product.Result;
                        if (GetColors.isSuccess)
                        {
                            ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName", product.Result.ColorId);
                        }
                        ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName", product.Result.CatogryId);

                        if (GetUnits.isSuccess)
                        {
                            ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName", product.Result.Unit);
                        }
                        ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString(), Selected = _keys.TransportMethodByme() == product.Result.TransportMethodId },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString(), Selected = _keys.TransportMethodByCompany() ==  product.Result.TransportMethodId  }
                };
                    }
                    else
                        return RedirectToAction(nameof(HomesController.UnAuthorized), "Homes");

                }
            }
            return View(viewModel);
        }
         [HttpPost]
        public async Task<IActionResult> CreateProduct_qta3a(ProductDto model, List<IFormFile> ProductImg)
        {
            try
            {
                var viewModel = new Product_backendDto();
                var CheckExistByCode = await _Product.CheckExistByCode(_httpContextAccessor.HttpContext, model.barcode, _keys.CatType_qta3a());

                var GetProduct = await _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId);
                if (GetProduct.isSuccess)
                {

                    viewModel = new Product_backendDto
                    {
                        ExistingItems = null,
                        NewItem = GetProduct.Result
                    };
                }
                if (model.ApearInHomePage != true)
                {
                    model.ApearInHomePage = false;
                }
                else
                    model.ApearInHomePage = true;
                if (model.SpecialOrder != true)
                {
                    model.SpecialOrder = false;
                }
                else
                    model.SpecialOrder = true;
                
                if (!ModelState.IsValid || (CheckExistByCode.isSuccess && model.ProductId == 0)||(model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null)))
                {
                    var GetCatogry = _Catogry.GetSubCatogry(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
                    if (GetCatogry.isSuccess)
                    {
                        ViewBag.CatogryId = new SelectList(GetCatogry.Result, "CatogryId", "CatogryName");
                    }
                    var GetColors = _Colors.GetColors().Result;
                    if (GetColors.isSuccess)
                    {
                        ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
                    }
                    var GetUnits = _Units.GetUnits().Result;
                    if (GetUnits.isSuccess)
                    {
                        ViewBag.Unit = new SelectList(GetUnits.Result, "UnitId", "UnitName");
                    }
                    if (CheckExistByCode.isSuccess && model.ProductId == 0)
                    {
                        ViewBag.check = "الكود مكرر ";
                    }
                    ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };

                    if (model.CatogryId == null)
                    {
                        ViewBag.CatogryIdMSG = "من فضلك اختار الصنف";
                    }
                    if (model.SubCatogryTitle == null)
                    {
                        ViewBag.SubCatogryTitle = "من فضلك ادخل العنوان الرئيسي";
                    }
                    if (model.barcode == null)
                    {
                        ViewBag.barcode = "من فضلك ادخل الكود";
                    }
                    if (model.BeforeDiscount == null)
                    {
                        ViewBag.Price = "من فضلك ادخل السعر";
                    }
                    if (model.BeforeDiscount < model.AfterDiscount && model.AfterDiscount != null)
                    {
                        ViewBag.AfterDiscount = "يجب ان يكون السعر اقل";
                    }
                    if (model.Unit == null)
                    {
                        ViewBag.Units = "من فضلك ادخل الوحدة";
                    }
                    if (model.QuantityAvailable == null)
                    {
                        ViewBag.QuantityAvailable = "من فضلك ادخل الكمية المتاحة";
                    }
                    if (model.MinQuantityToShipJomla == null)
                    {
                        ViewBag.MinQuantityToShipJomla = "من فضلك ادخل اقل كمية يمكن للمستخدم شراؤها";
                    }

                    if (model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                    {
                        ViewBag.ShippingPrice = "من فضلك ادخل سعر الشحن";
                    }
                    return View(viewModel);
                }
                else
                {
                    model.CatTypeId = _keys.CatType_qta3a();
                    if (model.ProductId != 0)
                    {
                        //var CheckPrices = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId, GetCatogryByRole.Result).Result;

                        //if (CheckPrices.isSuccess)
                        //{
                        if (!GetProduct.Result.AfterDiscount.Equals(model.AfterDiscount) || !GetProduct.Result.BeforeDiscount.Equals(model.BeforeDiscount) || !GetProduct.Result.discountBeginDate.Equals(model.discountBeginDate) || !GetProduct.Result.DiscountEndDate.Equals(model.DiscountEndDate))
                        {
                            var LogPriceDto = new LogPriceDto();
                            LogPriceDto.discountBeginDate = GetProduct.Result.discountBeginDate;

                            LogPriceDto.discountEndDat = GetProduct.Result.DiscountEndDate;
                            LogPriceDto.AfterDiscount = GetProduct.Result.AfterDiscount;
                            LogPriceDto.BeforeDiscount = GetProduct.Result.BeforeDiscount;

                            LogPriceDto.ProductId = GetProduct.Result.ProductId;
                            var LogPrices = _LogPrice.SaveLogPrice(_httpContextAccessor.HttpContext, LogPriceDto).Result;
                            if (LogPrices.isSuccess)
                            {

                            }
                            else
                            {
                                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                            }
                        }
                        if (model.QuantityAdded != 0)
                        {
                            var LogQuantityDto = new LogQuantityDto();
                            LogQuantityDto.NewQuantity = model.QuantityAdded;

                            LogQuantityDto.RemainingQuantity = GetProduct.Result.QuantityAvailable;
                            LogQuantityDto.ProductId = GetProduct.Result.ProductId;
                            var LogQuantity = _LogQuantity.SaveLogQuantity(_httpContextAccessor.HttpContext, LogQuantityDto).Result;
                            if (LogQuantity.isSuccess)
                            {

                            }
                            else
                            {
                                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                            }
                        }
                        ////}

                    }
                    if (model.ProductId != 0)
                    {
                        model.QuantityAvailable = model.QuantityAdded + GetProduct.Result.QuantityAvailable;
                    }

                    int ProductID = _Product.SaveProductInsideSite(_httpContextAccessor.HttpContext, model).Result;
                    ReturnDto<bool> ProductImgReturn;
                    if (ProductImg.Count != 0)
                    {
                        ProductImgReturn = _ProductImg.AddFiles(_httpContextAccessor.HttpContext, ProductImg, ProductID).Result;
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
                    else
                    {
                        ViewBag.SuccessMessage = "";
                        // ModelState.Clear();
                    }
                    if (model.ProductId == 0)
                    {
                        var LogQuantityDto = new LogQuantityDto();
                        LogQuantityDto.NewQuantity = model.QuantityAvailable;

                        LogQuantityDto.RemainingQuantity = 0;
                        LogQuantityDto.ProductId = ProductID;
                        var LogQuantity = _LogQuantity.SaveLogQuantity(_httpContextAccessor.HttpContext, LogQuantityDto).Result;
                        if (LogQuantity.isSuccess)
                        {

                        }
                        else
                        {
                            ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                        }
                      
                        if (model.ShippingPrice == null && model.ShippingPriceDto.Count != 0)
                        {
                            //عشان بيبعت null with row
                            var f = model.ShippingPriceDto.FirstOrDefault().VarPrice;
                            if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null) && model.ShippingPriceDto.Count() == 1)
                            {
                                if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                            }
                            else
                            {


                                ReturnDto<bool> ShippingPriceReturn = await _ShippingPrice.SaveShippingPrice(_httpContextAccessor.HttpContext, model.ShippingPriceDto,false, ProductID);
                                if (ShippingPriceReturn.isSuccess)
                                {
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                                    ModelState.Clear();
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للسعر المتغير!";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "";
                            // ModelState.Clear();
                        }
                    }
                    else
                        ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                    if (model.ProductId != 0)
                    {
                        return RedirectToAction("Product_qta3a", new { ProductId = ProductID });
                    }
                    else
                        return RedirectToAction("Product_qta3a");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ !";
                return null;
            }
        }

        public async Task<IActionResult> Product_gomla_Added(int? ProductId)
        {
            //var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;

            var viewModel = new List<ApiEditProductDto>();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetCatogry = await _Catogry.GetItemInfoAsync(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), token);
                if (GetCatogry.isSuccess)
                {
                    ViewBag.CatogryId = new SelectList(GetCatogry.Result, "id", "name");
                }
                var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), null);

                if (GetAllProduct.isSuccess)
                {
                    var GetAllProductID = GetAllProduct.Result.Select(l => l.ApiProdID).OrderBy(id => id).ToList();
                    var GetProductByListProductApi = await _Product.GetProductByListProductApi(_httpContextAccessor.HttpContext, _keys.CatType_gomla(),token, GetAllProductID,0); 
                    if (GetProductByListProductApi.isSuccess)
                    {
                        foreach (var localProduct in GetProductByListProductApi.Result)
                        {
                            var apiProduct = GetAllProduct.Result
                                .FirstOrDefault(p => p.ApiProdID == localProduct.p_id);
                            if (apiProduct != null)
                            {
                                viewModel.Add(new ApiEditProductDto
                                {
                                    Description = apiProduct.Description, // Get description from localProduct
                                    title = localProduct.title, // Get title from apiProduct
                                                              //barcode = apiProduct.barcode, // Get barcode from apiProduct
                                    ApiProdID = apiProduct.p_id,
                                    p_id = localProduct.p_id,
                                    //                     title = item.title,
                                    //                     price = item.price,
                                    //                     barcode = item.barcode,
                                    //                     stock = item.stock,
                                                        ProductId= apiProduct.ProductId

                                });
                            }
                        }
                        return View(viewModel);
                    }
                }

                 
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Product(int? ProductId)
        {
            var viewModel = new List<ApiEditProductDto>();
            //var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetCatogry = await _Catogry.GetItemInfoAsync(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), token);
                if (GetCatogry.isSuccess)
                {
                    ViewBag.CatId = new SelectList(GetCatogry.Result, "id", "name");
                }
            }
                    var GetProduct = await _Product.GetProductApi(token);
            //    foreach (var item in GetProduct.Result)
            //    {
            //        var GetSingleProducts = _Product.GetSingleProducts(_httpContextAccessor.HttpContext, item.p_id, GetCatogryByRole.Result).Result;
            //        if (GetSingleProducts.isSuccess)
            //        {
            //            if (GetSingleProducts.Result != null)
            //            {
            //                 var productDto = new ApiEditProductDto
            //                {
            //                     ApiProdID = item.p_id,
            //                     p_id = item.p_id,
            //                     title = item.title,
            //                     price = item.price,
            //                     barcode = item.barcode,
            //                     stock = item.stock,
            //                     ProductId= GetSingleProducts.Result.ProductId

            //                 };
            //                viewModel.Add(productDto); 



            //            }
            //            else
            //            {
            //                var productDto = new ApiEditProductDto
            //                {
            //                    ApiProdID = item.p_id,
            //                    p_id = item.p_id,
            //                    title = item.title,
            //                    price = item.price,
            //                    barcode = item.barcode,
            //                    stock = item.stock,

            //                };
            //                viewModel.Add(productDto);
            //            }
            //        }
            //    } 
            if (GetProduct.isSuccess)
            {
                viewModel = GetProduct.Result;
            }
            else
                viewModel = null;
        

                return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> addToSite(int p_id,int unit_Id, string Unit_Name, int tax_ID, string SubCatogryTitle, string barcode)
        {
            try
            {
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;

                var viewModel = new ApiEditProductDto();
                viewModel.ApiProdID = p_id;
                viewModel.tax_ID = tax_ID;
                viewModel.barcode = barcode;
                viewModel.Unit_Name= Unit_Name; 
                viewModel.unit_Id = unit_Id;
                if (p_id != 0)
                {
                    var CheckExistProducts = _Product.CheckExistProducts(_httpContextAccessor.HttpContext, p_id, _keys.CatType_gomla(), GetCatogryByRole.Result).Result;

                    if (CheckExistProducts.isSuccess)
                    {
                        if (CheckExistProducts.Result==true)
                        {
                            viewModel.CatTypeId = _keys.CatType_gomla();
                            viewModel.MinQuantityToShipJomla  = 100; viewModel.unit_Id = unit_Id;
                            viewModel.SubCatogryTitle = SubCatogryTitle; viewModel.Unit_Name = Unit_Name;
                            int SaveProduct = _Product.SaveProduct(_httpContextAccessor.HttpContext, viewModel).Result;
                            if (SaveProduct != 0)
                            {
                                return Json(new { success = true, message = "تم الحفظ بنجاح :)!" });
                            }
                            else
                            {
                                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "تم اضافته للمتجر من قبل!" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
                    }
                }
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ!" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> addToSiteQta3(int p_id, string Unit_Name, int unit_Id, int tax_ID, string SubCatogryTitle)
        {
            try
            {
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
                var viewModel = new ApiEditProductDto();
                viewModel.ApiProdID = p_id;
                viewModel.tax_ID = tax_ID;
                viewModel.unit_Id = unit_Id;
                if (p_id != 0)
                {
                    var CheckExistProducts = _Product.CheckExistProducts(_httpContextAccessor.HttpContext, p_id, _keys.CatType_qta3a(), GetCatogryByRole.Result).Result;

                    if (CheckExistProducts.isSuccess)
                    {
                        if (CheckExistProducts.Result == true)
                        {
                            viewModel.CatTypeId = _keys.CatType_qta3a();
                            viewModel.MaxQuantityToShipQta3a = 1;
                            viewModel.SubCatogryTitle = SubCatogryTitle;
                            viewModel.unit_Id = unit_Id; viewModel.Unit_Name = Unit_Name;
                            int SaveProduct = _Product.SaveProduct(_httpContextAccessor.HttpContext, viewModel).Result;
                            if (SaveProduct != 0)
                            {
                                return Json(new { success = true, message = "تم الحفظ بنجاح :)!" });
                            }
                            else
                            {
                                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "تم اضافته للمتجر من قبل!" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
                    }
                }
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ للمنتج!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "حدثت مشكلة اثناء الحفظ!" });
            }
        }

        public async Task<IActionResult> Edit_Product(int? p_id,int ProductId)
        { 
            ViewBag.ProductId= ProductId;
            var viewModel = new ApiEditProductDto();
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
             var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetProduct = await _Product.GetProductByIDApi(token, p_id.Value);
                if (GetProduct.isSuccess)
                {
                    viewModel = GetProduct.Result;
                    //viewModel.title = GetProduct.Result.title;
                    //viewModel.barcode = GetProduct.Result.barcode;
                    //viewModel.barcode = GetProduct.Result.barcode;
                    //viewModel.stock = GetProduct.Result.stock;
                    //viewModel.price = GetProduct.Result.price;
                    //viewModel.unites.FirstOrDefault().unitName = GetProduct.Result.unites.FirstOrDefault().unitName;

                    var product = _Product.GetSingleProducts(_httpContextAccessor.HttpContext, ProductId, GetCatogryByRole.Result).Result;
                    if (product.isSuccess)
                    {
                        if(product.Result!=null)
                        {

                       
                     viewModel = product.Result;
                            //viewModel.SubCatogryTitle = product.Result.SubCatogryTitle;
                            //viewModel.Description = product.Result.Description;
                            //viewModel.MaxQuantityToShipQta3a = product.Result.MaxQuantityToShipQta3a;
                            //viewModel.MinQuantityToShipJomla = product.Result.MinQuantityToShipJomla;
                            //viewModel.ColorId = product.Result.ColorId;
                            //viewModel.ApearInHomePage = product.Result.ApearInHomePage;
                            //viewModel.SpecialOrder = product.Result.SpecialOrder;
                            //viewModel.AfterDiscount = product.Result.AfterDiscount;
                            //viewModel.discountBeginDate = product.Result.discountBeginDate;
                            //viewModel.DiscountEndDate = product.Result.DiscountEndDate;
                            //viewModel.ShippingPrice = product.Result.ShippingPrice;
                                viewModel.ProductId = ProductId;
                            viewModel.title = GetProduct.Result.title;
                            viewModel.barcode = GetProduct.Result.barcode;
                            viewModel.barcode = GetProduct.Result.barcode;
                            viewModel.stock = GetProduct.Result.stock;
                            viewModel.price = GetProduct.Result.price;
                            viewModel.unit_Id = GetProduct.Result.unit_Id;
                            viewModel.tax_ID = GetProduct.Result.tax_ID;
                            viewModel.unites1 = GetProduct.Result.unites.FirstOrDefault().unitName;

                        }
                    }
                }
                else
                    viewModel = null;
            }
            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }
             
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Product(ApiEditProductDto model, List<IFormFile> ProductImg)
        {
            try
            {
                var viewModel = new ApiEditProductDto();
                var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_gomla()).Result;
                
                    if (model.ApearInHomePage != true)
                    {
                        model.ApearInHomePage = false;
                    }
                    else
                        model.ApearInHomePage = true;
                    if (model.SpecialOrder != true)
                    {
                        model.SpecialOrder = false;
                    }
                    else
                        model.SpecialOrder = true;
                    if (!ModelState.IsValid)
                    {

                        var GetColors = _Colors.GetColors().Result;
                        if (GetColors.isSuccess)
                        {
                            ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
                        }

                        ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };
                        if (model.MinQuantityToShipJomla == null)
                        {
                            ViewBag.MinQuantityToShipJomla = "من فضلك ادخل اقل كمية يمكن للمستخدم شراؤها";
                        }

                        if (model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                        {
                            ViewBag.ShippingPrice = "من فضلك ادخل سعر الشحن";
                        }
                        return View(viewModel);
                    }
                    else
                    {

                        if (model.ProductId != 0)
                        {
                            var CheckPrices = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId ).Result;

                            if (CheckPrices.isSuccess)
                            {
                                if (!CheckPrices.Result.AfterDiscount.Equals(model.AfterDiscount) || !CheckPrices.Result.discountBeginDate.Equals(model.discountBeginDate) || !CheckPrices.Result.DiscountEndDate.Equals(model.DiscountEndDate))
                                {
                                    var LogPriceDto = new LogPriceDto();
                                    LogPriceDto.discountBeginDate = model.discountBeginDate;
                                    LogPriceDto.discountEndDat = model.DiscountEndDate;
                                    LogPriceDto.AfterDiscount = model.AfterDiscount;
                                     LogPriceDto.BeforeDiscount = double.Parse(model.price.ToString());
                                    LogPriceDto.ProductId = CheckPrices.Result.ProductId;
                                    var LogPrices = _LogPrice.SaveLogPrice(_httpContextAccessor.HttpContext, LogPriceDto).Result;
                                    if (LogPrices.isSuccess)
                                    {

                                    }
                                    else
                                    {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                                    }
                                }


                            }

                        }
                        model.CatTypeId = _keys.CatType_gomla();
                    model.ApiProdID = model.p_id;
                      if (model.ShippingPrice != null)
                    {
                        model.ShippingPriceDto  = null;
                    }
                        int SaveProduct = _Product.SaveProduct(_httpContextAccessor.HttpContext, model).Result;

                        if (SaveProduct != 0)
                        {

                            ReturnDto<bool> ProductImgReturn;
                            if (ProductImg.Count != 0)
                            {
                                ProductImgReturn = _ProductImg.AddFiles(_httpContextAccessor.HttpContext, ProductImg, SaveProduct).Result;
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
                            else
                            {
                            ViewBag.SuccessMessage = "";
                                // ModelState.Clear();
                            }
                            if (model.ShippingPrice == null && model.ShippingPriceDto.Count != 0)
                            {
                                //عشان بيبعت null with row
                                var f = model.ShippingPriceDto.FirstOrDefault().VarPrice;
                                if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null) && model.ShippingPriceDto.Count() == 1)
                                {
                                    if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                                }
                                else
                                {


                                    ReturnDto<bool> ShippingPriceReturn = await _ShippingPrice.SaveShippingPrice(_httpContextAccessor.HttpContext, model.ShippingPriceDto,false, SaveProduct);
                                    if (ShippingPriceReturn.isSuccess)
                                    {
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                                        ModelState.Clear();
                                    }
                                    else
                                    {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للسعر المتغير!";
                                    }
                                }
                            }
                            else if (model.ShippingPrice != null)
                        {
                            ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                        }
                        
                        else
                            {
                            ViewBag.ErrorMessage = "";
                                // ModelState.Clear();
                            }
                        }
                        else
                        ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                        if (model.ProductId != 0)
                        {
                            return RedirectToAction("Edit_Product", new { ProductId = model.ProductId, p_id = model.p_id });
                        }
                        else
                            return RedirectToAction("Product_gomla_Added");
                    }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ !";
                return null;
            }
        }
        public ActionResult EditShippingPrice( int id)
        {
            ViewBag.id = id;
            var check=_Product.CheckShippingCost(_httpContextAccessor.HttpContext, id).Result;
            if (check.isSuccess)
            {
                ViewBag.check = check.Result;
            }
            var GetProduct = _ShippingPrice.GetShippingPrice(_httpContextAccessor.HttpContext, id).Result;
            List<ShippingPriceDto> viewModel = new List<ShippingPriceDto>();
            if (GetProduct.isSuccess)
            {
                viewModel = GetProduct.Result;
            }
            // Retrieve IsODD_Even from the first item in the list (assuming all have the same value)
            if (viewModel.Any())
            {
                ViewBag.IsODD_Even = viewModel.First().IsODD_Even;
            }
            return View(viewModel);
        } 
        // POST: ShippingPriceController/Edit/5
      
        [HttpPost]
        public async Task<ActionResult> EditShippingPriceAsync(int id, bool IsODD_Even, List<ShippingPriceDto> shippingPriceDto)
        {
            if (ModelState.IsValid)
            {
                ViewBag.IsODD_Even = IsODD_Even;

                if ((shippingPriceDto.Any(sp => sp.VarPrice == null || sp.VarQuantity == null) && !IsODD_Even) &&
    (IsODD_Even && shippingPriceDto.Any(sp => sp.VarPrice == null || sp.VarQuantity == null)))
                {
                    ViewBag.ErrorMessage = "يجب إدخال السعر والكمية.";
                }

                else
                {
                    var ShippingPriceReturn = await _ShippingPrice.SaveShippingPrice(_httpContextAccessor.HttpContext, shippingPriceDto, IsODD_Even, id);

                    if (ShippingPriceReturn.isSuccess)
                    {
                        ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                        return RedirectToAction("EditShippingPrice", new { id = id });
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "حدثت مشكلة أثناء الحفظ!";
                    }
                }
            }

            List<ShippingPriceDto> viewModel = new List<ShippingPriceDto>();
            var GetProduct = _ShippingPrice.GetShippingPrice(_httpContextAccessor.HttpContext, id).Result;

            if (GetProduct.isSuccess)
            {
                viewModel = GetProduct.Result;
            }
            ViewBag.IsODD_Even = IsODD_Even;
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult DeleteImg(int id)
        {
            var GetProduct = _ProductImg.DeleteFile(_httpContextAccessor.HttpContext, id).Result;
            bool viewModel ;
            if (GetProduct.isSuccess)
            {
                viewModel = GetProduct.Result;
            }
            return Json("تم الحذف");
        }
        [HttpPost]
        public JsonResult DeleteShippingPrice(int id)
        {
            var GetProduct = _ShippingPrice.DeleteShippingPrice(_httpContextAccessor.HttpContext, id).Result;
            bool viewModel;
            if (GetProduct.isSuccess)
            {
                viewModel = GetProduct.Result;
            }
            return Json("تم الحذف");
        }
        [HttpPost]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            var Getimg = _ProductImg.DeleteAllFile(_httpContextAccessor.HttpContext, id).Result;
            if (Getimg.isSuccess)
            {
            }
            var GetShippingPrice = _ShippingPrice.DeleteAllShippingPrice(_httpContextAccessor.HttpContext, id).Result;
            if (GetShippingPrice.isSuccess)
            {
            }
            var GetProduct =await _Product.DeleteProducts(_httpContextAccessor.HttpContext, id);

            if (GetProduct.isSuccess)
            {
                return Json("تم الحذف");
            }


            return Json("");
        }


        public async Task<IActionResult> Edit_Product_qta3a(int? p_id, int ProductId)
        {
            ViewBag.ProductId = ProductId;
            var viewModel = new ApiEditProductDto();
            var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetProduct = await _Product.GetProductByIDApi(token, p_id.Value);
                if (GetProduct.isSuccess)
                {
                    viewModel = GetProduct.Result;
                    //viewModel.title = GetProduct.Result.title;
                    //viewModel.barcode = GetProduct.Result.barcode;
                    //viewModel.barcode = GetProduct.Result.barcode;
                    //viewModel.stock = GetProduct.Result.stock;
                    //viewModel.price = GetProduct.Result.price;
                    //viewModel.unites.FirstOrDefault().unitName = GetProduct.Result.unites.FirstOrDefault().unitName;

                    var product = _Product.GetSingleProducts(_httpContextAccessor.HttpContext, ProductId, GetCatogryByRole.Result).Result;
                    if (product.isSuccess)
                    {
                        if (product.Result != null)
                        {


                            viewModel = product.Result;
                            //viewModel.SubCatogryTitle = product.Result.SubCatogryTitle;
                            //viewModel.Description = product.Result.Description;
                            //viewModel.MaxQuantityToShipQta3a = product.Result.MaxQuantityToShipQta3a;
                            //viewModel.MinQuantityToShipJomla = product.Result.MinQuantityToShipJomla;
                            //viewModel.ColorId = product.Result.ColorId;
                            //viewModel.ApearInHomePage = product.Result.ApearInHomePage;
                            //viewModel.SpecialOrder = product.Result.SpecialOrder;
                            //viewModel.AfterDiscount = product.Result.AfterDiscount;
                            //viewModel.discountBeginDate = product.Result.discountBeginDate;
                            //viewModel.DiscountEndDate = product.Result.DiscountEndDate;
                            //viewModel.ShippingPrice = product.Result.ShippingPrice;
                            viewModel.ProductId = ProductId;
                            viewModel.title = GetProduct.Result.title;
                            viewModel.barcode = GetProduct.Result.barcode;
                            viewModel.barcode = GetProduct.Result.barcode;
                            viewModel.stock = GetProduct.Result.stock;
                            viewModel.price = GetProduct.Result.price;

                            viewModel.unit_Id = GetProduct.Result.unit_Id;
                            viewModel.tax_ID = GetProduct.Result.tax_ID;
                            viewModel.unites1 = GetProduct.Result.unites.FirstOrDefault().unitName;

                        }
                    }
                }
                else
                    viewModel = null;
            }
            var GetColors = _Colors.GetColors().Result;
            if (GetColors.isSuccess)
            {
                ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
            }

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit_Product_qta3a(ApiEditProductDto model, List<IFormFile> ProductImg)
        {
            try
            {
                var viewModel = new ApiEditProductDto();
                 var GetCatogryByRole = _Catogry.GetCatogryByRole(_httpContextAccessor.HttpContext, _keys.CatType_qta3a()).Result;

                if (model.ApearInHomePage != true)
                {
                    model.ApearInHomePage = false;
                }
                else
                    model.ApearInHomePage = true;
                if (model.SpecialOrder != true)
                {
                    model.SpecialOrder = false;
                }
                else
                    model.SpecialOrder = true;
                if (!ModelState.IsValid)
                {

                    var GetColors = _Colors.GetColors().Result;
                    if (GetColors.isSuccess)
                    {
                        ViewBag.ColorId = new SelectList(GetColors.Result, "ColorId", "ColorName");
                    }

                    ViewBag.TransportMethodId = new List<SelectListItem>
                {
                   // new SelectListItem { Text = "Select User Type", Value = "-1" }, // Default empty option
                    new SelectListItem { Text = _keys.TransportMethodByme_text().ToString(), Value = _keys.TransportMethodByme().ToString() },
                    new SelectListItem { Text = _keys.TransportMethodByCompany_text().ToString(), Value = _keys.TransportMethodByCompany().ToString() }
                };
                    if (model.MaxQuantityToShipQta3a == null)
                    {
                        ViewBag.MinQuantityToShipJomla = "من فضلك ادخل اكبر كمية يمكن للمستخدم شراؤها";
                    }

                    if (model.ShippingPrice == null && model.ShippingPriceDto.Count == 1 && (model.ShippingPriceDto.FirstOrDefault().VarPrice == null && model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                    {
                        ViewBag.ShippingPrice = "من فضلك ادخل سعر الشحن";
                    }
                    return View(viewModel);
                }
                else
                {

                    if (model.ProductId != 0)
                    {
                        var CheckPrices = _Product.GetSingleProduct(_httpContextAccessor.HttpContext, model.ProductId ).Result;

                        if (CheckPrices.isSuccess)
                        {
                            if (!CheckPrices.Result.AfterDiscount.Equals(model.AfterDiscount) || !CheckPrices.Result.discountBeginDate.Equals(model.discountBeginDate) || !CheckPrices.Result.DiscountEndDate.Equals(model.DiscountEndDate))
                            {
                                var LogPriceDto = new LogPriceDto();
                                LogPriceDto.discountBeginDate = model.discountBeginDate;
                                LogPriceDto.discountEndDat = model.DiscountEndDate;
                                LogPriceDto.AfterDiscount = model.AfterDiscount;
                                LogPriceDto.BeforeDiscount = double.Parse(model.price.ToString());
                                LogPriceDto.ProductId = CheckPrices.Result.ProductId;
                                var LogPrices = _LogPrice.SaveLogPrice(_httpContextAccessor.HttpContext, LogPriceDto).Result;
                                if (LogPrices.isSuccess)
                                {

                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                                }
                            }


                        }

                    }
                    model.CatTypeId = _keys.CatType_qta3a();
                    model.ApiProdID = model.p_id;
                    int SaveProduct = _Product.SaveProduct(_httpContextAccessor.HttpContext, model).Result;

                    if (SaveProduct != 0)
                    {

                        ReturnDto<bool> ProductImgReturn;
                        if (ProductImg.Count != 0)
                        {
                            ProductImgReturn = _ProductImg.AddFiles(_httpContextAccessor.HttpContext, ProductImg, SaveProduct).Result;
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
                        else
                        {
                            ViewBag.SuccessMessage = "";
                            // ModelState.Clear();
                        }
                        if (model.ShippingPrice == null && model.ShippingPriceDto.Count != 0)
                        {
                            //عشان بيبعت null with row
                            var f = model.ShippingPriceDto.FirstOrDefault().VarPrice;
                            if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null) && model.ShippingPriceDto.Count() == 1)
                            {
                                if ((model.ShippingPriceDto.FirstOrDefault().VarPrice == null || model.ShippingPriceDto.FirstOrDefault().VarQuantity == null))
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                            }
                            else
                            {


                                ReturnDto<bool> ShippingPriceReturn = await _ShippingPrice.SaveShippingPrice(_httpContextAccessor.HttpContext, model.ShippingPriceDto,false, SaveProduct);
                                if (ShippingPriceReturn.isSuccess)
                                {
                                    ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                                    ModelState.Clear();
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للسعر المتغير!";
                                }
                            }
                        }
                        else if (model.ShippingPrice != null)
                        {
                            ViewBag.SuccessMessage = "تم الحفظ بنجاح :)!";
                        }

                        else
                        {
                            ViewBag.ErrorMessage = "";
                            // ModelState.Clear();
                        }
                    }
                    else
                        ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ للمنتج!";
                    if (model.ProductId != 0)
                    {
                        return RedirectToAction("Edit_Product_qta3a", new { ProductId = model.ProductId, p_id = model.p_id });
                    }
                    else
                        return RedirectToAction("Product_qta3a_Added");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = "حدثت مشكلة اثناء الحفظ !";
                return null;
            }
        }


    }
}
