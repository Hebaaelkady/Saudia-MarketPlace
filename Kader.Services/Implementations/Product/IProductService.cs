using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Product;
using Kader.DTOs.ReturnsOrderItem;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Product
{

    public interface IProductService
    {
        Task<ReturnDto<int>> ProductCount(HttpContext context, int? type);
        Task<ReturnDto<List<HomeProductDto>>> GetAllProductThatQuantityLess(HttpContext context,int? type, int? QuantityAvailable);
        Task<int> SaveProductInsideSite(HttpContext context, ProductDto Product);
        Task<ReturnDto<ProductDto>> GetSingleProduct(HttpContext context, int ProductId );
        Task<ReturnDto<bool>> DeleteProducts(HttpContext context, int id);
        Task<ReturnDto<bool>> checkProductByCatID(HttpContext context, List<int> id); 
        Task<int> SaveProduct(HttpContext context, ApiEditProductDto Product);
        Task<ReturnDto<List<ProductViewDto>>> GetAllProduct(HttpContext context, int? type, List<int> GetCatogryByRole);
        Task<ReturnDto<bool>> CheckShippingCost(HttpContext context, int ProductId);
        Task<ReturnDto<bool>> DeleteAllProducts(HttpContext context, int id);
        Task<ReturnDto<List<ProductViewDto>>> GetAllDeletedProduct(HttpContext context, int? type);
        Task<ReturnDto<bool>> RestoreDeleteProducts(HttpContext context, int id);
        Task<ReturnDto<bool>> CheckExistByCode(HttpContext context, string code, int catType);
        Task<ReturnDto<T>> GetSingleProduct<T>(int ProductId) where T : class, new();
        Task<ReturnDto<bool>> IncreaseQuantity(List<IncreaseReturnsOrderItemDto> OrderItems);
        Task<ReturnDto<List<ApiEditProductDto>>> GetProductsByIdsApi(string token, List<int> productIds);
        Task<ReturnDto<List<Product_frontEndDto>>> GetAllProduct(int? type);
        Task<ReturnDto<List<T>>> GetProductsByCategoryser<T>(List<int> categoryIds, int type) where T : class, new();
        Task<ReturnDto<List<T>>> GetProductsByprodID<T>(List<int> ProductsID) where T : class, new();
        Task<ReturnDto<bool>> DecreaseQuantity(  List<OrderItemsDto> OrderItems);
        Task<ReturnDto<bool>> RestoreQuantity(HttpContext context, IList<Itemslist> OrderItems);
        Task<List<OrderItemsDto>> GetUnavailableProductsAsync(List<OrderItemsDto> orderItems);
            Task<bool> CheckStockAvailabilityAsync(List<Itemslist> orderItems);

        Task<ReturnDto<List<HomeVarDto>>> GoodPrice(int? type, int? take);
        Task<ReturnDto<List<HomeVarDto>>> SpecialOrder(int? type, int? take);
        Task<ReturnDto<List<ApiEditProductDto>>> GetProductApi(string token);
        Task<ReturnDto<List<ApiEditProductDto>>> GetProductByCatApi(string token,  int  categoryIds);
        Task<ReturnDto<ApiEditProductDto>> GetProductByIDApi(string token, int productId);
        Task<ReturnDto<ApiEditProductDto>> GetSingleProducts(HttpContext context, int ProductId, List<int> GetCatogryByRole);
        Task<ReturnDto<List<ApiEditProductDto>>> GetProductByListProductApi(HttpContext context, int type, string token, List<int> prodid, int categoryId);
        Task<ReturnDto<List<ApiEditProductDto>>> GetAllProducts(HttpContext context, int? type, List<int> GetCatogryByRole);
        Task<ReturnDto<bool>> CheckExistProducts(HttpContext context, int ApiProdId, int CatTypeId, List<int> GetCatogryByRole);
        Task<ReturnDto<List<ApiEditProductDto>>> GetProductByCatListApi(string token, List<int> categoryId);
        Task<ReturnDto<List<ApiEditProductDto>>> GetAllProductsByListOfProd(List<int> ProductID,int CatTypeId);
        Task<ReturnDto<List<SearchProductDto>>> SearchProductsAsync(string searchTerm);
    }
}
