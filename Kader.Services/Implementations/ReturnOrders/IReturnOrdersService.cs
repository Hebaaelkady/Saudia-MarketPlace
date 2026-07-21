using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Kader.DTOs.Orderss;

namespace Kader.Services.Implementations.ReturnOrders
{

    public interface IReturnOrdersService
    {
        Task<ReturnDto<List<T>>> GetReturnsOrder<T>(HttpContext context) where T : class, new();
        Task<ReturnDto<int>> SaveReturnsOrder(HttpContext context, saveReturnsOrdersDto OrderssDto);
        Task<ReturnDto<bool>> DeleteReturnsOrder(HttpContext context, int id);
        Task<ReturnDto<int>> updateStatus(HttpContext context, int Status, int orderID);
        //Task<ReturnDto<ReturnsOrdersDto>> GetSingleReturnsOrder(int id);
        Task<ReturnDto<ReturnsOrdersDto>> GetReturnsOrder(int orderId, List<ItemslistOrderDto> orderItems);
        Task<ReturnDto<List<T>>> GetAllReturnsOrderByStatus<T>(HttpContext context, int status) where T : class, new();
        Task<ReturnDto<ReturnsOrdersDto>> GetReturnsOrderWithItems (HttpContext context, int ReturnsOrderId) ;
        Task<ReturnDto<T>> GetSingleReturnsOrder<T>(HttpContext context, int orderId) where T : class, new();
        Task<ReturnDto<int>> Update (HttpContext context, AcceptReturnsOrdersDto dto, int ReturnsOrderId) ;
        Task<ReturnDto<Dictionary<int, int>>> GetCountOrdersByStatuses(HttpContext context, List<int> statuses);
        Task<ReturnDto<bool>> update(HttpContext context, AssignStoreOrderDto AssignStoreOrderDto);
    }
}
