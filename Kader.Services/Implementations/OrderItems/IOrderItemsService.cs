using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.CatType;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.OrderItems
{

    public interface IOrderItemsService
    {
        Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems();
        Task<ReturnDto<bool>> SaveOrderItems(HttpContext context,    IList<Itemslist> OrderItems,int OrderId);
        Task<ReturnDto<bool>> DeleteOrderItems(HttpContext context, int id);
        Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems(string id);
        Task<ReturnDto<List<OrderItemsDto>>> GetDeletedOrderItems();
        Task<ReturnDto<bool>> RestoreDeleteOrderItems(HttpContext context, int id);
        Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems(int typeGomlaOrQt3a);
    }
}
