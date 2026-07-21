using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.OrderStatus;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.ReturnsOrderStatus
{

    public interface IReturnsOrderStatusService
    {
        Task<ReturnDto<List<OrderStatusDto>>> GetOrderStatus(int orderid);
        Task<ReturnDto<bool>> DeleteOrderStatus(HttpContext context, int id);
        Task<ReturnDto<bool>> SaveOrderStatus(HttpContext context, int OrderID, int statusID, int StoreId);
        Task<ReturnDto<OrderStatusDto>> GetSingleOrderStatus(int id);
        Task<ReturnDto<List<OrderStatusDto>>> GetDeletedOrderStatus(); 
    }
}
