using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.ReturnsOrderItem;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.ReturnsOrderItem
{

    public interface IReturnsOrderItemService
    {
        // Task<ReturnDto<List<ReturnsOrderItemDto>>> GetReturnsOrderItem(HttpContext context);
        Task<ReturnDto<bool>> SaveReturnsOrder(HttpContext context, IList<ReturnsOrderItemDto> OrderItems, int OrderId);
        Task<ReturnDto<List<IncreaseReturnsOrderItemDto>>> GetReturnsOrderItems(int id);
    }
}
