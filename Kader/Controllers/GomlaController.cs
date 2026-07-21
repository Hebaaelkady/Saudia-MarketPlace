
using DocumentFormat.OpenXml.Vml;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Middlewares.Cookies;
using Kader.Services.Implementations.Catogry;
using Kader.Services.Implementations.Product;
using Kader.Services.Implementations.ProductImg;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using DocumentFormat.OpenXml.Bibliography;
using Kader.Services.Implementations.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kader.Infrastructure.Jwt;
namespace Kader.Controllers
{

    
    public class GomlaController : Controller
    {
        private IAuthenticateService _userService;
        private IHttpContextAccessor _httpContextAccessor; private readonly ICookies _cookies;
        private ICatogryService _Catogry; private IProductImgService _ProductImg;
        private IProductService _Product; private IKeys _keys;
        public List<CatogryDto> Gov_codesDetail { get; set; }
        public GomlaController(IAuthenticateService userService, ICatogryService CatogryService, IHttpContextAccessor httpContextAccessor, ICookies cookies, IProductImgService ProductImgService, IProductService ProductService )
        {
            _userService = userService;
            _Catogry = CatogryService; _Product = ProductService; _ProductImg = ProductImgService;
            _httpContextAccessor = httpContextAccessor; _cookies = cookies; _keys = new Keys();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string categoryId3, int? page, string sortOrder, int? id)
        {
            var viewModel = new ProductView_front();
             
                var GetCatogry = await _Catogry.GetCatogry<ApiCatogryDto>(_httpContextAccessor.HttpContext, _keys.CatType_gomla() );
                if (GetCatogry.isSuccess)
                {
                    var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), null);
                    if (GetAllProduct.isSuccess)
                    {
                    viewModel.Products =GetAllProduct.Result;
                }


                    viewModel.ApiCatogryDto = GetCatogry.Result;
                }
                else
                {
                    viewModel.ApiCatogryDto = null;
                }
                // Get all products from your repository
            
            ViewBag.id = id;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> IndexApi(string categoryId3,int? page, string sortOrder,int? id)
        {
            var viewModel = new  ProductView_front();
            var token = await _userService.GetTokenAsync();
            if (token != null)
            {
                var GetCatogry = await _Catogry.GetItemInfoAsync(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), token);
                if (GetCatogry.isSuccess)
                {
                    var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), null);
                    if (GetAllProduct.isSuccess)
                    {
                        var GetAllProductID = GetAllProduct.Result.Select(l => l.ApiProdID).OrderBy(id => id).ToList();
                        var GetProductByListProductApi = await _Product.GetProductByListProductApi(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), token, GetAllProductID, 0);
                        if (GetProductByListProductApi.isSuccess)
                        {
                            foreach (var localProduct in GetProductByListProductApi.Result)
                            {
                                var apiProduct = GetAllProduct.Result
                                    .FirstOrDefault(p => p.ApiProdID == localProduct.p_id);

                                if (apiProduct != null)
                                {
                                    viewModel.Products.Add(new ApiEditProductDto
                                    {
                                        Description = apiProduct.Description, // Get description from localProduct
                                        title = localProduct.title,
                                        ApiProdID = localProduct.p_id,
                                        p_id = localProduct.p_id,
                                        price = localProduct.price,
                                        MinQuantityToShipJomla = apiProduct.MinQuantityToShipJomla,
                                        CatTypeId = apiProduct.CatTypeId,
                                        stock = localProduct.stock,
                                        ProductId = apiProduct.ProductId,
                                        AfterDiscount = apiProduct.AfterDiscount,
                                        Image = apiProduct.Image,
                                        categories= localProduct.categories,
                                    });
                                }
                            }
                        }
                    }

               
                    viewModel.ApiCatogryDto = GetCatogry.Result;
                }
                else
                {
                    viewModel.ApiCatogryDto = null;
                }
                // Get all products from your repository
            } 
            ViewBag.id = id;
        
            return  View(viewModel);
        }
        [HttpGet]
       
        public async Task<IActionResult> GetProductsByCategory(  string categoryId3, int? page, string sortOrder, string id)
        {
            var viewModel = new ProductView_front(); List<int> categoryIdsList = new List<int>();
            if (categoryId3 ==null &&(id==null|| id == "0"))
            {
                 
                    var GetCatogry =   await _Catogry.GetCatogry<ApiCatogryDto>(_httpContextAccessor.HttpContext, _keys.CatType_gomla());
                    if (GetCatogry.isSuccess)
                    {
                        ViewBag.CatId = new SelectList(GetCatogry.Result, "id", "name");
                    }
                    var GetAllProduct = await _Product.GetAllProducts(_httpContextAccessor.HttpContext, _keys.CatType_gomla(), null);

                    if (GetAllProduct.isSuccess)
                    {

                    viewModel.Products = GetAllProduct.Result;
                                 
                            int pageSize = 6; // Set your desired page size
                            int currentPage = page.HasValue && page.Value > 0 ? page.Value : 1;
                            int totalPages = (int)Math.Ceiling((double)viewModel.Products.Count / pageSize);
                            // Sort Order
                            sortOrder = string.IsNullOrEmpty(sortOrder) ? "price_asc" : sortOrder; // Default sort order

                            // Sorting
                            var sortedProducts = new List<Product_frontEndDto>();
                            if (sortOrder == "price_asc")
                            {
                                viewModel.Products = viewModel.Products.OrderBy(p => p.price).ToList();
                            }
                            else if (sortOrder == "price_desc")
                            {
                                viewModel.Products = viewModel.Products.OrderByDescending(p => p.price).ToList();
                            }
                            else
                            if (sortOrder == "title_asc")
                            {
                                viewModel.Products = viewModel.Products.OrderBy(p => p.title).ToList();
                            }
                            else if (sortOrder == "title_desc")
                            {
                                viewModel.Products = viewModel.Products.OrderByDescending(p => p.title).ToList();
                            }

                            // Pagination
                            var pagedProducts = viewModel.Products.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(); //return Json(new { success = true, products = products });

                            return Json(new { success = true, products = pagedProducts, currentPage, totalPages });

                         
                    }

                
                return View(viewModel);
            }
          
            else if (!string.IsNullOrEmpty(categoryId3)||(id!=null||id!="0"))
            {
                if (id != null || id != "0")
                {
                    if (categoryId3 == null)
                    {
                        categoryId3 = id;
                    }
                    else
                    {
                        categoryId3 += ","+ id.ToString() ;
                    }
                }

                categoryIdsList = categoryId3.Split(',')
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(int.Parse).Distinct()
                    .ToList();
                
                       var GetProductByCatListApi = await _Product.GetProductsByCategoryser<ApiEditProductDto>( categoryIdsList, _keys.CatType_gomla()); // Pass Type directly
                        if (GetProductByCatListApi.isSuccess)
                        {
                        
                            viewModel.Products = GetProductByCatListApi.Result;
                             
                                int pageSize = 6; // Set your desired page size
                                int currentPage = page.HasValue && page.Value > 0 ? page.Value : 1;
                                int totalPages = (int)Math.Ceiling((double)viewModel.Products.Count / pageSize);
                                // Sort Order
                                sortOrder = string.IsNullOrEmpty(sortOrder) ? "price_asc" : sortOrder; // Default sort order

                                // Sorting
                                var sortedProducts = new List<Product_frontEndDto>();
                            if (sortOrder == "price_asc")
                            {
                                viewModel.Products = viewModel.Products.OrderBy(p => p.price).ToList();
                            }
                            else if (sortOrder == "price_desc")
                            {
                                viewModel.Products = viewModel.Products.OrderByDescending(p => p.price).ToList();
                            }
                            else
                            if (sortOrder == "title_asc")
                                {
                                    viewModel.Products = viewModel.Products.OrderBy(p => p.title).ToList();
                                }
                                else if (sortOrder == "title_desc")
                                {
                                    viewModel.Products = viewModel.Products.OrderByDescending(p => p.title).ToList();
                                }

                                // Pagination
                                var pagedProducts = viewModel.Products.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(); //return Json(new { success = true, products = products });

                                return Json(new { success = true, products = pagedProducts, currentPage, totalPages });

                          

                        }

                    
                 
            }
            

          
            else
            {
                 
                return View(viewModel);
            }
            return View(viewModel);
        }

        public async Task<IActionResult> details(int? id)
        {
            var viewModel = new ProductView_front();

            if (id.HasValue)
            {
                
                    var product = await _Product.GetSingleProduct<ApiEditProductDto>( id.Value);
                    if (product.isSuccess)
                    {
                        viewModel.Productsitem = product.Result;
                        return View(viewModel);
                    }
                 
            }
            return View(null);
        }
        [HttpGet]
        public async Task<IActionResult> SearchGomla(string searchTerm)
        {
            var result =await _Product.SearchProductsAsync(searchTerm); // Replace with your search logic
            if (result.isSuccess)
            {
                return Json(new { products = result.Result });
            }

            return Json(new { products = new List<SearchProductDto>() });
        }
        [HttpGet]
        public async Task<IActionResult> SearchQt3a(string searchTerm)
        {
            var result = await _Product.SearchProductsAsync(searchTerm); // Replace with your search logic
            if (result.isSuccess)
            {
                return Json(new { products = result.Result });
            }

            return Json(new { products = new List<SearchProductDto>() });
        }


    }
}
