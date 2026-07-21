using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.CatType;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Orderss;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Order
{

    public interface IOrderssService
    {
        Task<ReturnDto<bool>> updateSms(HttpContext context, MsgDto AssignStoreOrderDto);
        Task<ReturnDto<Dictionary<int, int>>> GetCountOrdersByStatuses(HttpContext context, List<int> statuses);
        //Task<ReturnDto<List<OrderssDto>>> GetOrder(HttpContext context);
        Task<ReturnDto<int>> SaveOrder(HttpContext context, SaveCheckoutDto OrderDto);
        Task<ReturnDto<bool>> DeleteOrder(HttpContext context, int id);
        //Task<ReturnDto<Checkout_frontDto>> GetSingleOrder(HttpContext context, int id);
        Task<ReturnDto<List<OrderssDto>>> GetDeletedOrder();
        Task<ReturnDto<bool>> SaveOrderApi(string token, Checkout_frontDto order);
        Task<ReturnDto<List<OrderBackendDto>>> GetAllOrder(HttpContext context);
        Task<ReturnDto<List<OrderBackendDto>>> GetAllOrderByStatus(HttpContext context,int status);
        Task<ReturnDto<bool>> updateStatus(HttpContext context, int Status, int orderID);
        Task<ReturnDto<bool>> updateStatusWithPayID(  int Status, string PaymentId); 
        Task<ReturnDto<int>> updateshipment(int Idorders,string otoId, DateTime? DeliveryDate, DateTime? ShippingDate, string success,int status,string InsideShippingUser);
        Task<ReturnDto<T>> GetSingleOrder<T>(HttpContext context, string id) where T : class, new();
        Task<ReturnDto<List<T>>> GetOrder<T>(HttpContext context) where T : class, new();
        Task<ReturnDto<List<T>>> GetOrderNotMoreThan14Day<T>(HttpContext context) where T : class, new();
        Task<ReturnDto<bool>> update(HttpContext context, AssignStoreOrderDto AssignStoreOrderDto);
        Task<ReturnDto<bool>> GetOrderNotMoreThan14DayStatus1And3(HttpContext context);
        Task<ReturnDto<bool>> CheckCode(HttpContext context, string id);
        Task<ReturnDto<List<OrderssDto>>> GetAllOrderNotSendMsgToUser(HttpContext context);
        Task<ReturnDto<List<OrderssDto>>> GetAllOrderNotSendMsgToDelivery(HttpContext context);
    }
}
