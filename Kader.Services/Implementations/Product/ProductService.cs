using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Utilities.Mappers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using Kader.DTOs.OrderItems;
using Kader.DTOs.ReturnsOrderItem;
namespace Kader.Services.Implementations.Product
{

    public class ProductService : IProductService
    {
        private HttpClient _httpClient;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; private IKeys _keys;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly string outSideUrl = "https://jinaapi.auditor.sa/Items/Info";
        private readonly string outSideUrl1 = "https://jinaapi.auditor.sa/Items";
        public ProductService(HttpClient httpClient, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork; _keys = new Keys();
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        public async Task<ReturnDto<int>> ProductCount(HttpContext context, int? type )
        {
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            //var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            try
            {
                List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == type) ?? new List<Data.DataAccessLayer.Entities.Product>();
                
                return new ReturnDto<int>(true, getData.Count(), string.Empty);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<int>(false, 0, ex.Message);
            }
        }
        public async Task<ReturnDto<ProductDto>> GetSingleProduct(HttpContext context, int ProductId )
        {
            var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;

            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            var actIDpiCattypeList = context.User.Claims
     .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)) // Filter out null or empty values
     .Select(c =>
     {
         if (int.TryParse(c.Value, out var value)) // Safely parse the value
         {
             return (int?)value; // Return a nullable int
         }
         return null; // Return null for invalid values
     })
     .Where(v => v.HasValue) // Exclude null values
     .Select(v => v.Value)  // Convert back to non-nullable ints
     .ToList();
            try
            {
                if (isAdmin || TypeUser == null|| roleClaims.Contains(_keys.ProductRole()))
                {
                    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ProductId == ProductId, i1 => i1.ProductImg, i7 => i7.Catogry, i7 => i7.LogQuantity, i2 => i2.ShippingPriceNavigation, i4 => i4.UnitNavigation, i4 => i4.LogPrice);
                    var map = _mapper.Map<ProductDto>(getData);
                    return new ReturnDto<ProductDto>(true, map, string.Empty);
                }
                else
                {
                    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ProductId == ProductId && actIDpiCattypeList.Any(id => id == o.CatogryId.Value), i1 => i1.ProductImg, i7 => i7.Catogry, i2 => i2.ShippingPriceNavigation, i4 => i4.UnitNavigation, i4 => i4.LogPrice);

                    // List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == int.Parse(TypeID) && (message.Contains(o.CatogryId.Value)), i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    var map = _mapper.Map<ProductDto>(getData);
                    return new ReturnDto<ProductDto>(true, map, string.Empty);
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<ProductDto>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> CheckExistByCode(HttpContext context,string code,int catType)
        {
            try
            {
                var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.Barcode == code&&o.CatTypeId==catType, i1 => i1.ProductImg, i7 => i7.Catogry, i7 => i7.LogQuantity, i2 => i2.ShippingPriceNavigation, i4 => i4.UnitNavigation, i4 => i4.LogPrice);
                if (getData != null)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
                return new ReturnDto<bool>(false, false, "err"); 
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        private async Task<List<int>> CheckProductsInSubCategories(List<int> categoryIds)
        {
            IList<int> subCategories = new List<int>();
            foreach (var categoryId in categoryIds)
            {
                var getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && (o.CatId == categoryId));
                if (getData.Any()) // base case: if there are no more subcategories, stop the recursion
                {
                    var subCategoryIds = getData.Select(l => l.CatogryId).ToList();
                    var message = await CheckProductsInSubCategories(subCategoryIds);
                    subCategories = subCategories.Concat(message).ToList(); // combine the results of all recursive calls
                }
                subCategories.Add(categoryId);
            }
            return (List<int>)subCategories;
        }
        public async Task<ReturnDto<List<ProductViewDto>>> GetAllProduct(HttpContext context, int? type, List<int> GetCatogryByRole)
        {
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            //var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            var actIDpiCattypeList = context.User.Claims
    .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)) // Filter out null or empty values
    .Select(c =>
    {
        if (int.TryParse(c.Value, out var value)) // Safely parse the value
        {
            return (int?)value; // Return a nullable int
        }
        return null; // Return null for invalid values
    })
    .Where(v => v.HasValue) // Exclude null values
    .Select(v => v.Value)  // Convert back to non-nullable ints
    .ToList();

            try
            {
                if (isAdmin || roleClaims.Contains(_keys.ProductRole()))
                {
                    
                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false&&/*o.ApiProdId!=null &&*/ o.CatTypeId == type, i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                    var map = _mapper.Map<List<ProductViewDto>>(getData);
                    return new ReturnDto<List<ProductViewDto>>(true, map, string.Empty);
                }
                else
                {

                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(
     o => o.IsDeleted == false&&o.CatTypeId == type && o.CatogryId.HasValue && actIDpiCattypeList.Any(id => id == o.CatogryId.Value),
     i1 => i1.Catogry,
     i2 => i2.ShippingPriceNavigation,
     i3 => i3.ProductImg,
     i4 => i4.TransportMethod
 ) ?? new List<Data.DataAccessLayer.Entities.Product>();

                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());

                    var map = _mapper.Map<List<ProductViewDto>>(getData);
                    return new ReturnDto<List<ProductViewDto>>(true, map, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ProductViewDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<HomeProductDto>>> GetAllProductThatQuantityLess(HttpContext context, int? type, int? QuantityAvailable)
        {
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            //var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            try
            {
                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.QuantityAvailable <=QuantityAvailable && o.CatTypeId == type) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    var map = _mapper.Map<List<HomeProductDto>>(getData);
                    return new ReturnDto<List<HomeProductDto>>(true, map, string.Empty);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<HomeProductDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<List<ProductViewDto>>> GetAllDeletedProduct(HttpContext context, int? type)
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == true && o.CatTypeId == type, i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                var map = _mapper.Map<List<ProductViewDto>>(getData);
                return new ReturnDto<List<ProductViewDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ProductViewDto>>(false, null, ex.Message);
            }
        }


        public async Task<ReturnDto<bool>> checkProductByCatID(HttpContext context, List<int> id)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            try
            {

                List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false&&id.Contains(o.CatogryId.Value) ) ?? new List<Data.DataAccessLayer.Entities.Product>();
                if (getData.Count()!=0)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
                return new ReturnDto<bool>(false, false, "err");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        
        public async Task<ReturnDto<bool>> DeleteAllProducts(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatogryId == id) ?? new List<Data.DataAccessLayer.Entities.Product>();
                foreach (var Products in getData)
                {
                    Products.DeletedBy = stringUserId;
                    Products.IsDeleted = true;
                    Products.DeletedDate = DateTime.UtcNow.AddHours(3);
                    await _unitOfWork.Product.UpdateAsync(Products);
                }
                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> RestoreDeleteProducts(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Product.UpdateAsync(newCat);

                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> DeleteProducts(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Product.UpdateAsync(newCat);

                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> CheckShippingCost(HttpContext context, int ProductId)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            try
            {
                var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ProductId == ProductId && o.ShippingPrice==null);
                if(getData!=null)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
                return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        //frontend servics 
        //Product_frontEndDto
        public async Task<ReturnDto<T>> GetSingleProduct<T>( int ProductId) where T : class, new()
        {
            try
            {
                var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.QuantityAvailable >=1 && o.ProductId == ProductId, i1 => i1.ProductImg, i7 => i7.Catogry, i7 => i7.UnitNavigation, i2 => i2.ShippingPriceNavigation, i4 => i4.TransportMethod, i4 => i4.LogPrice, i4 => i4.CatType);
                var map = _mapper.Map<T>(getData);
                return new ReturnDto<T>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<T>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<Product_frontEndDto>>> GetAllProduct(int? type)
        
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == type,
                    i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod)
                    ?? new List<Data.DataAccessLayer.Entities.Product>();
                getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                var map = _mapper.Map<List<Product_frontEndDto>>(getData);
                return new ReturnDto<List<Product_frontEndDto>>(true, map, string.Empty);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<Product_frontEndDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<List<T>>> GetProductsByCategoryser<T>(List<int> categoryIds,int type) where T : class, new()
        {
            

                    // Efficiently filter products based on category IDs
                    var products = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && categoryIds.Contains(o.CatogryId.Value)&&o.CatTypeId==type&&o.QuantityAvailable>=1,k=>k.ProductImg);
            //Product_frontEndDto
            // Map to front-end DTO and return response
            var map = _mapper.Map<List<T>>(products);
            return new ReturnDto<List<T>>(true, map, string.Empty);
        }
        public async Task<ReturnDto<List<T>>> GetProductsByprodID<T>(List<int> ProductsID) where T : class, new()
        {
            //Product_frontEndDto
            // Efficiently filter products based on category IDs
            //  var products = await _unitOfWork.Product.FindAsync(o => c&& ProductsID.Contains(o.ProductId)&&o.ShippingPriceNavigation.Where(l=>l.IsDeleted==false), i => i.ProductImg, j => j.UnitNavigation, k =>k.Color,f=>f.ShippingPriceNavigation);

            var products = await _unitOfWork.Product.FindAsync(
                o =>  o.IsDeleted==false && ProductsID.Contains(o.ProductId),
                i => i.ProductImg,
                j => j.UnitNavigation,
                k => k.Color,
                f => f.ShippingPriceNavigation // No filtering here!
            );

            // Manually filter the related data **after** fetching it
            foreach (var product in products)
            {
                product.ShippingPriceNavigation = product.ShippingPriceNavigation
                    .Where(l =>l.IsDeleted==false)
                    .ToList();
            }
 

            // Map to front-end DTO and return response
            var map = _mapper.Map<List<T>>(products);
            return new ReturnDto<List<T>>(true, map, string.Empty);
        }
        public async Task<ReturnDto<List<ApiEditProductDto>>> GetProductsByIdsApi(string token, List<int> productIds)
        {
            try
            {
                var apiTasks = productIds.Select(async productId =>
                {
                    string requestUrl = $"{outSideUrl1}/{productId}";
                    var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
                    {
                        Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
                    };

                    using var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var jsonString = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<ApiEditProductDto>(jsonString);
                });

                // Await all tasks to complete
                var apiProducts = await Task.WhenAll(apiTasks);

                return new ReturnDto<List<ApiEditProductDto>>(true, apiProducts.ToList(), string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }
        public async Task<List<OrderItemsDto>> GetUnavailableProductsAsync(List<OrderItemsDto> orderItems)
        {
            var unavailableItems = new List<OrderItemsDto>();

            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Product
                    .SingleOrDefaultAsync(p => p.ProductId == item.ProductId)
                    ;

                if (product == null || product.QuantityAvailable <item.Quantity)
                {
                    unavailableItems.Add(item);
                }
            }

            return unavailableItems;
        }
        public async Task<bool> CheckStockAvailabilityAsync(List<Itemslist> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == item.ProductId);
                     
                if (product == null || product.QuantityAvailable <item.Quantity)
                {
                    return false; // Stock not available for one or more products
                }
            }
            return true; // Stock available for all products
        }
        public async Task<ReturnDto<bool>> IncreaseQuantity(List<IncreaseReturnsOrderItemDto> OrderItems)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {

                if (OrderItems != null)
                {
                    //List<Itemslist> updatedOrderItems = new List<Itemslist>(); // Create a new list

                    foreach (var OrderItemsDto in OrderItems)
                    {
                        try
                        {
                            var newCat = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == OrderItemsDto.ProductId);

                            if (newCat != null /*&& newCat.QuantityAvailable >= OrderItemsDto.Quantity*/)
                            {
                                newCat.QuantityAvailable += OrderItemsDto.Quantity;
                                await _unitOfWork.Product.UpdateAsync(newCat);
                            }
                            else
                            {
                                return new ReturnDto<bool>(false, false, "");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Kader.System.Error: {ex.Message}");
                            return new ReturnDto<bool>(false, false, ex.Message);
                        }
                    }

                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        await transaction.CommitAsync();
                        return new ReturnDto<bool>(true, true, string.Empty);
                    }

                    else
                    {
                        await transaction.RollbackAsync();
                        return new ReturnDto<bool>(false, false, " يعض العناصر المخزون لايكفي !");

                    }

                }

                return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> DecreaseQuantity(  List<OrderItemsDto> OrderItems)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                 
                if (OrderItems != null)
                {
                    List<Itemslist> updatedOrderItems = new List<Itemslist>(); // Create a new list

                    foreach (var OrderItemsDto in OrderItems)
                    {
                        try
                        {
                            var newCat = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == OrderItemsDto.ProductId);

                            if (newCat != null /*&& newCat.QuantityAvailable >= OrderItemsDto.Quantity*/)
                            {
                                newCat.QuantityAvailable -= OrderItemsDto.Quantity;
                                await _unitOfWork.Product.UpdateAsync(newCat);
                               // updatedOrderItems.Add(OrderItemsDto); // Add valid item to the new list
                            }
                            else
                            {
                                return new ReturnDto<bool>(false, false,"");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Kader.System.Error: {ex.Message}");
                            return new ReturnDto<bool>(false, false, ex.Message);
                        }
                    }

                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        await transaction.CommitAsync();
                        return new ReturnDto<bool>(true, true, string.Empty);
                    }
                        
                    else
                    { await transaction.RollbackAsync();
                        return new ReturnDto<bool>(false, false, " يعض العناصر المخزون لايكفي !");
                       
                    }
                    
                }

                return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> RestoreQuantity(HttpContext context, IList<Itemslist> OrderItems)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (OrderItems != null)
                {
                    foreach (var OrderItemsDto in OrderItems)
                    {
                        try
                        {
                            var newCat = await _unitOfWork.Product.SingleOrDefaultAsync(l => l.ProductId == OrderItemsDto.ProductId);

                            if (newCat != null && newCat.QuantityAvailable >= OrderItemsDto.Quantity)
                            {

                                newCat.QuantityAvailable = newCat.QuantityAvailable + OrderItemsDto.Quantity;
                                await _unitOfWork.Product.UpdateAsync(newCat);
                            }
                            //else
                            //{
                            //    return new ReturnDto<bool>(false, false, "المخزون لايكفي !");
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Kader.System.Error: {ex.Message}");
                            return new ReturnDto<bool>(false, false, ex.Message);
                        }

                    }
                    if (await _unitOfWork.CompleteAsync() > 0)
                        return new ReturnDto<bool>(true, true, string.Empty);
                    else
                        return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
                }

                return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<List<HomeVarDto>>> SpecialOrder(int? type,  int? take)
        {
            try
            {
                // Validate input parameters
                 
                if (type == null || take == null || take <= 0)
                {
                    return new ReturnDto<List<HomeVarDto>>(false, null, "Invalid type or take value");
                }

                List<Data.DataAccessLayer.Entities.Product> getData = null;
                    getData = (await _unitOfWork.Product.FindAsync(
                        o => o.IsDeleted == false && o.QuantityAvailable>= 1 && o.ApearInHomePage == true && o.CatTypeId == type,
                        i1 => i1.ProductImg,
                        i7 => i7.Catogry,
                        i2 => i2.ShippingPriceNavigation
                        //i4 => i4.TransportMethod,
                        //i4 => i4.LogPrice
                    )).Take(take.Value).ToList();
              
                
           
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<HomeVarDto>>(false, null, "No data found");
                }

                var map = _mapper.Map<List<HomeVarDto>>(getData);
                return new ReturnDto<List<HomeVarDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<HomeVarDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<HomeVarDto>>> GoodPrice(int? type,   int? take)
        {
            try
            {
                 
                if (type == null || take == null || take <= 0)
                {
                    return new ReturnDto<List<HomeVarDto>>(false, null, "Invalid type or take value");
                }

                List<Data.DataAccessLayer.Entities.Product> getData = null;

                 
                    getData = (await _unitOfWork.Product.FindAsync(
                        o => o.IsDeleted == false && o.QuantityAvailable >= 1 && o.AfterDiscount != null && o.CatTypeId == type,
                        i1 => i1.ProductImg,
                        i7 => i7.Catogry,
                        i2 => i2.ShippingPriceNavigation 
                        
                    )).Take(take.Value).ToList();
                
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<HomeVarDto>>(false, null, "No data found");
                }

                var map = _mapper.Map<List<HomeVarDto>>(getData);
                return new ReturnDto<List<HomeVarDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<HomeVarDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<List<ApiEditProductDto>>> GetProductApi(string token)
        {
            try
            {
                var model = new ReturnDto<List<ApiEditProductDto>>(true, null, string.Empty);
                Uri myUri = new Uri(outSideUrl, UriKind.Absolute);
                var request = new HttpRequestMessage(HttpMethod.Get, myUri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using(_httpClient = new HttpClient())
                {
                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // Deserialize into a list of ApiProductDto 
                    var apiProductDtos = JsonConvert.DeserializeObject<List<ApiEditProductDto>>(jsonString);
                    model = new ReturnDto<List<ApiEditProductDto>>(true, apiProductDtos, string.Empty);
                }

                // No mapping is needed here
                // Return the deserialized ApiProductDtos directly 
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<ApiEditProductDto>>> GetProductByCatApi(string token, int categoryId)
        {
            try
            {
                Uri myUri = new Uri($"{outSideUrl}?categoryId={categoryId}", UriKind.Absolute);
                var request = new HttpRequestMessage(HttpMethod.Get, myUri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
               
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize into a list of ApiProductDto 
                var apiProductDtos = JsonConvert.DeserializeObject<List<ApiEditProductDto>>(jsonString);

                // Filter the list by the single categoryId
                var filteredProducts = apiProductDtos
                    .Where(p => p.categories.Contains(categoryId))
                    .ToList();

                // Return the filtered list
                return new ReturnDto<List<ApiEditProductDto>>(true, filteredProducts, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<ApiEditProductDto>>> GetProductByCatListApi(string token, List<int> categoryId)
        {
            try
            {
                var categoryIdsString = string.Join(",", categoryId);
                Uri myUri = new Uri($"{outSideUrl}?categoryId={categoryIdsString}", UriKind.Absolute);
                var request = new HttpRequestMessage(HttpMethod.Get, myUri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
              
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize into a list of ApiProductDto 
                var apiProductDtos = JsonConvert.DeserializeObject<List<ApiEditProductDto>>(jsonString);

                // Filter the list by the single categoryId
                var filteredProducts = apiProductDtos
            .Where(p => p.categories.Any(c => categoryId.Contains(c)))
            .ToList();

                 
                // Return the filtered list
                return new ReturnDto<List<ApiEditProductDto>>(true, filteredProducts, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<ApiEditProductDto>>> GetProductByListProductApi(HttpContext context,int type, string token,  List<int> prodid, int categoryId)
        {
            try
            {
                bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
                var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;

                var actIDpiCattypeList = context.User.Claims
    .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)) // Filter out null or empty values
    .Select(c =>
    {
        if (int.TryParse(c.Value, out var value)) // Safely parse the value
        {
            return (int?)value; // Return a nullable int
        }
        return null; // Return null for invalid values
    })
    .Where(v => v.HasValue) // Exclude null values
    .Select(v => v.Value)  // Convert back to non-nullable ints
    .ToList();

                Uri myUri = new Uri(outSideUrl, UriKind.Absolute);
                var request = new HttpRequestMessage(HttpMethod.Get, myUri);
               // var request = new HttpRequestMessage(HttpMethod.Get, $"https://jinaapi.auditor.sa/Items/Info");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
              
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize into a list of ApiProductDto 
                var apiProductDtos = JsonConvert.DeserializeObject<List<ApiEditProductDto>>(jsonString);
                List<ApiEditProductDto> filteredProducts = new List<ApiEditProductDto>();
                if (categoryId==0)
                {
                    if(isAdmin || TypeUser == null || TypeUser == "2")
                    {

                    
                      filteredProducts = apiProductDtos.Where(p => prodid.Contains(p.p_id) ).ToList();
                    
                    }
                    else
                    {
                   //     var actIDpiCattypePairs = actIDpiCattypeList
                   //.Select(claim => claim.Split(':'))
                   //.Select(parts => new { actIDpi = int.Parse(parts[0]), cattype = int.Parse(parts[1]) })
                   //.ToList();
                   //     var filteredPairs = actIDpiCattypePairs.Where(pair => pair.cattype == type).ToList();

                   //     filteredProducts = apiProductDtos
                   //         .Where(p => prodid.Contains(p.p_id) && filteredPairs.Any(pair => p.categories.Contains(pair.actIDpi)))
                   //         .ToList();

                    }
                }
                else
                {
                      filteredProducts = apiProductDtos
    .Where(p => prodid.Contains(p.p_id) && p.categories.Contains(categoryId))
    .ToList();
                }
                // Filter the list by the single categoryId
               


                // Return the filtered list
                return new ReturnDto<List<ApiEditProductDto>>(true, filteredProducts, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<ApiEditProductDto>> GetProductByIDApi(string token, int productId)
        {
            try
            {
                string requestUrl = $"{outSideUrl1}/{productId}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
                {
                    Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer", token)
            }
                };

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserialize using Newtonsoft.Json.JsonConvert
                var apiProductDto = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiEditProductDto>(jsonString);

                return new ReturnDto<ApiEditProductDto>(true, apiProductDto, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<ApiEditProductDto>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<ApiEditProductDto>> GetSingleProducts(HttpContext context, int ProductId, List<int> GetCatogryByRole)
        {
            var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;
            
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            try
            {
                //if (isAdmin|| TypeUser == null)
                //{
                    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ProductId == ProductId, i1 => i1.ProductImg, i7 => i7.Catogry, i2 => i2.ShippingPriceNavigation, i4 => i4.TransportMethod, i4 => i4.LogPrice);
                    var map = _mapper.Map<ApiEditProductDto>(getData);
                    return new ReturnDto<ApiEditProductDto>(true, map, string.Empty);
                //}
                //else
                //{
                //    List<int> myList = new List<int> { int.Parse(CatogryID) };
                //    var message = await CheckProductsInSubCategories(GetCatogryByRole);
                //    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ProductId == ProductId && message.Contains(o.CatogryId.Value), i1 => i1.ProductImg, i7 => i7.Catogry, i2 => i2.ShippingPriceNavigation, i4 => i4.TransportMethod, i4 => i4.LogPrice);

                //    // List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == int.Parse(TypeID) && (message.Contains(o.CatogryId.Value)), i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                //    var map = _mapper.Map<ApiEditProductDto>(getData);
                //    return new ReturnDto<ApiEditProductDto>(true, map, string.Empty);
                //}
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<ApiEditProductDto>(false, null, ex.Message);
            }
        }
        public async Task<int> SaveProduct(HttpContext context, ApiEditProductDto Product)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (Product.ProductId == 0)
                {
                    var newCat = _mapper.Map<Data.DataAccessLayer.Entities.Product>(Product);
                    newCat.InsertedBy = stringUserId;
                    await _unitOfWork.Product.AddAsync(newCat);
                    if (await _unitOfWork.CompleteAsync() > 0)
                        return newCat.ProductId;
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    var oldCat = await _unitOfWork.Product.GetAsync(Product.ProductId);
                    //Product.ShippingPriceNavigation =  oldCat.ShippingPriceNavigation  ;
                    var newCat = _mapper.Map(Product, oldCat);
                    newCat.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat.UpdateBy = stringUserId;
                    //newCat.ShippingPriceNavigation = oldCat.ShippingPriceNavigation;
                    await _unitOfWork.Product.UpdateAsync(newCat);


                    if (await _unitOfWork.CompleteAsync() > 0)
                        return oldCat.ProductId;
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return 0;
            }
        }
        public async Task<int> SaveProductInsideSite(HttpContext context, ProductDto Product)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (Product.ProductId == 0)
                {
                    var newCat = _mapper.Map<Data.DataAccessLayer.Entities.Product>(Product);
                    newCat.InsertedBy = stringUserId;
                    await _unitOfWork.Product.AddAsync(newCat);
                    if (await _unitOfWork.CompleteAsync() > 0)
                        return newCat.ProductId;
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    var oldCat = await _unitOfWork.Product.GetAsync(Product.ProductId);
                    //Product.ShippingPriceNavigation =  oldCat.ShippingPriceNavigation  ;
                    var newCat = _mapper.Map(Product, oldCat);
                    newCat.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat.UpdateBy = stringUserId;
                    //newCat.ShippingPriceNavigation = oldCat.ShippingPriceNavigation;
                    await _unitOfWork.Product.UpdateAsync(newCat);


                    if (await _unitOfWork.CompleteAsync() > 0)
                        return oldCat.ProductId;
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return 0;
            }
        }
        public async Task<ReturnDto<bool>> CheckExistProducts(HttpContext context, int ApiProdId, int CatTypeId, List<int> GetCatogryByRole)
        {
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            try
            {
                if (isAdmin)
                {
                    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ApiProdId == ApiProdId&&o.CatTypeId== CatTypeId);
                    //var map = _mapper.Map<ApiEditProductDto>(getData);
                    if(getData==null)
                    return new ReturnDto<bool>(true, true, "غير موجود");
                    else
                        return new ReturnDto<bool>(true, false, "موجود");
                }
                else
                {
                    List<int> myList = new List<int> { int.Parse(CatogryID) };
                    var message = await CheckProductsInSubCategories(GetCatogryByRole);
                    var getData = await _unitOfWork.Product.SingleOrDefaultAsync(o => o.IsDeleted == false && o.ApiProdId == ApiProdId && o.CatTypeId == CatTypeId && message.Contains(o.CatogryId.Value), i1 => i1.ProductImg, i7 => i7.Catogry, i2 => i2.ShippingPriceNavigation, i4 => i4.TransportMethod, i4 => i4.LogPrice);

                    // List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == int.Parse(TypeID) && (message.Contains(o.CatogryId.Value)), i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();

                    if (getData == null)
                        return new ReturnDto<bool>(true, true, "غير موجود");
                    else
                        return new ReturnDto<bool>(true, false, "موجود");
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<List<ApiEditProductDto>>> GetAllProducts(HttpContext context, int? type, List<int> GetCatogryByRole)
        {
            var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            try
            {
                if (isAdmin)
                {

                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false &&   o.CatTypeId == type, i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                    getData.ForEach(p => p.ProductImg = p.ProductImg.Where(k => !k.IsDeleted.Value).ToList());
                    var map = _mapper.Map<List<ApiEditProductDto>>(getData);
                    return new ReturnDto<List<ApiEditProductDto>>(true, map, string.Empty);
                }
                else if ( (TypeUser  != "2"&& roleClaims.Count()>0))
                {
                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false &&   o.CatTypeId == type, i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                    getData.ForEach(p => p.ProductImg = p.ProductImg.Where(k => !k.IsDeleted.Value).ToList());

                    var map = _mapper.Map<List<ApiEditProductDto>>(getData);
                    return new ReturnDto<List<ApiEditProductDto>>(true, map, string.Empty);
                }
                else if (TypeUser == null || TypeUser=="2")
                {
                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.CatTypeId == type&&o.QuantityAvailable>=1, i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                    getData.ForEach(p => p.ProductImg = p.ProductImg.Where(k => !k.IsDeleted.Value).ToList());

                    var map = _mapper.Map<List<ApiEditProductDto>>(getData);
                    return new ReturnDto<List<ApiEditProductDto>>(true, map, string.Empty);
                }
                else
                {
                    //   List<int> myList = new List<int> { int.Parse(CatogryID) };
                    var message = await CheckProductsInSubCategories(GetCatogryByRole);

                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false &&   (message.Contains(o.CatogryId.Value)), i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());

                    var map = _mapper.Map<List<ApiEditProductDto>>(getData);
                    return new ReturnDto<List<ApiEditProductDto>>(true, map, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ApiEditProductDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<List<ApiEditProductDto>>> GetAllProductsByListOfProd( List<int> ProductID, int CatTypeId)
        {
              

                    List<Data.DataAccessLayer.Entities.Product> getData = await _unitOfWork.Product.FindAsync(o => o.IsDeleted == false && o.ApiProdId != null && o.CatTypeId == CatTypeId && ProductID.Contains(o.ApiProdId.Value), i1 => i1.Catogry, i2 => i2.ShippingPriceNavigation, i3 => i3.ProductImg, i4 => i4.TransportMethod) ?? new List<Data.DataAccessLayer.Entities.Product>();
                    getData.ForEach(p => p.ShippingPriceNavigation = p.ShippingPriceNavigation.Where(k => !k.IsDeleted.Value).ToList());
                    var map = _mapper.Map<List<ApiEditProductDto>>(getData);
                    return new ReturnDto<List<ApiEditProductDto>>(true, map, string.Empty);
                 
        }
        public async Task<ReturnDto<List<SearchProductDto>>> SearchProductsAsync(string searchTerm)
        {
            // Fetch and filter the data from the database
            var getData = await _unitOfWork.Product.FindAsync(o =>
                o.IsDeleted == false &&o.QuantityAvailable>=1 &&
                o.SubCatogryTitle.Contains(searchTerm))
                ?? new List<Data.DataAccessLayer.Entities.Product>();

            var filteredData = getData
                .Select(o => new SearchProductDto
                {
                    ProductId = o.ProductId, 
                    CatTypeId = o.CatTypeId.Value,
                    SubCatogryTitle = o.SubCatogryTitle
                }).ToList();

            return new ReturnDto<List<SearchProductDto>>(true, filteredData, string.Empty);
        }



    }
}
