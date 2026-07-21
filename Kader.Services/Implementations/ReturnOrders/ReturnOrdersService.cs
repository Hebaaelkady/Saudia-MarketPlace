using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.CatType;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;
using Kader.Infrastructure.Shared.Interfaces;
using System.Net.Http;
using Kader.Infrastructure.Shared.Implementations;
using System.Security.Claims;
using System.Security.Cryptography;
using Kader.DTOs.Orderss;
namespace Kader.Services.Implementations.ReturnOrders
{

    public class ReturnOrdersService : IReturnOrdersService
    {
        private IUnitOfWork _unitOfWork; private IKeys _keys;
        private IMapper _mapper; private readonly HttpClient _httpClient;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ReturnOrdersService(HttpClient httpClient, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork; _httpClient = httpClient; _keys = new Keys();
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        public async Task<ReturnDto<List<T>>> GetAllReturnsOrderByStatus<T>(HttpContext context, int status) where T : class, new()
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var roleClaims = context.User.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList();

                var typeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;
                var Stores = context.User.Claims.Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList();
                List<ReturnsOrder> getData;
 
                    if (roleClaims.Contains(_keys.storeRole()))
                    {


                        // Query for restricted users
                        getData = await _unitOfWork.ReturnsOrder.FindAsync(
                            o =>   Stores.Any(id => id == o.StoreId.ToString()) &&

                                o.StatusId == status,
                i1 => i1.Order.Address,
                i2 => i2.Order, i2 => i2.Order.OrderItems,
                i3 => i3.ReturnsOrderItem,
                i4 => i4.Status
                        ) ?? new List<ReturnsOrder>();
                    }
                    else
                    {
                        // Ensure the data retrieval returns a list of Orderss entities
                         getData = await _unitOfWork.ReturnsOrder.FindAsync(
                o => o.StatusId == status,
                i1 => i1.Order.Address,
                i2 => i2.Order, i2 => i2.Order.OrderItems,
                i3 => i3.ReturnsOrderItem,
                i4 => i4.Status
            ) ?? new List<ReturnsOrder>();
                    }
                // Check if getData is of the expected type
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<T>>(false, null, "No orders found.");
                }

                // Map the list of Orderss entities to a list of OrderBackendDto
                var map = _mapper.Map<List<T>>(getData);

                return new ReturnDto<List<T>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<T>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> update(HttpContext context, AssignStoreOrderDto AssignStoreOrderDto)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            Data.DataAccessLayer.Entities.ReturnsOrder newCat1 = null;
            if (AssignStoreOrderDto.Idorders != 0)
            {
                //AssignStoreOrderDto.ReturnsOrderId = AssignStoreOrderDto.Idorders;
                var oldCat = await _unitOfWork.ReturnsOrder.GetAsync(AssignStoreOrderDto.Idorders);
                newCat1 = _mapper.Map(AssignStoreOrderDto, oldCat);

                await _unitOfWork.ReturnsOrder.UpdateAsync(newCat1);


                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        //int y = newCat1.Idorders;
                        return new ReturnDto<bool>(true, true, string.Empty);
                    }
                    else
                    {
                        return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                    return new ReturnDto<bool>(false, false, ex.Message);
                }
            }

            return new ReturnDto<bool>(false, false, "OrderssDto is null");
        }

        //  هفير يغخdtoReturnsOrdersDto
        public async Task<ReturnDto<List<T>>> GetReturnsOrder<T>(HttpContext context) where T : class, new()
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.ReturnsOrder>? getData = await _unitOfWork.ReturnsOrder.FindAsync(d => d.InsertedBy == stringUserId, u => u.Order, u => u.Store, u => u.Order.OrderItems, u => u.ReturnsOrderItem, u => u.Status, u => u.ReturnsOrderStatus) ?? new List<Data.DataAccessLayer.Entities.ReturnsOrder>();
                var map = _mapper.Map<List<T>>(getData);
                return new ReturnDto<List<T>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<T>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<int>> SaveReturnsOrder(HttpContext context, saveReturnsOrdersDto OrderssDto)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (OrderssDto == null || OrderssDto.ReturnsOrderItem == null || OrderssDto.ReturnsOrderItem.Count == 0)
            {
                return new ReturnDto<int>(false, 0, "No items selected for the return order.");
            }
            else
            {
                Data.DataAccessLayer.Entities.ReturnsOrder newCat1 = null;


                newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.ReturnsOrder>(OrderssDto);
                newCat1.InsertedBy = stringUserId;


                await _unitOfWork.ReturnsOrder.AddAsync(newCat1);



                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        int y = newCat1.ReturnsOrderId;
                        return new ReturnDto<int>(true, y, string.Empty);
                    }
                    else
                    {
                        return new ReturnDto<int>(false, 0, "Not Saved, Error Occurred!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                    return new ReturnDto<int>(false, 0, ex.Message);
                }
            }
             
        }

    
        public async Task<ReturnDto<bool>> DeleteReturnsOrder(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.ReturnsOrder.SingleOrDefaultAsync(l => l.ReturnsOrderId == id);

                await _unitOfWork.ReturnsOrder.RemoveAsync(newCat);

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
        //for update
        public async Task<ReturnDto<ReturnsOrdersDto>> GetReturnsOrder(int orderId, List<ItemslistOrderDto> orderItems)
        {
            try
            {
                // Fetch return orders associated with the order
                var existingReturns = await _unitOfWork.ReturnsOrder
                    .FindAsync(l => l.OrderId == orderId, i => i.Order, u => u.ReturnsOrderItem);

                if (existingReturns == null)
                {
                    return new ReturnDto<ReturnsOrdersDto>(false, null, "Nothing found!");
                }

                // Check for each item in orderItems if it has an existing return
                foreach (var item in orderItems)
                {
                    item.HasExistingReturn = existingReturns.Any(returnOrder =>
                        returnOrder.ReturnsOrderItem.Any(roi => roi.OrderItemsId == item.OrderItemsId));
                }

                // Map the return orders to DTO
                var map = _mapper.Map<ReturnsOrdersDto>(existingReturns);
                map.Itemslist = orderItems; // Include the updated items list

                return new ReturnDto<ReturnsOrdersDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<ReturnsOrdersDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<int>> updateStatus(HttpContext context, int Status, int orderID)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (orderID != 0)
            {

                var oldCat = await _unitOfWork.ReturnsOrder.GetAsync(orderID);

                oldCat.StatusId = Status;
                await _unitOfWork.ReturnsOrder.UpdateAsync(oldCat);


                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        //int y = newCat1.Idorders;
                        return new ReturnDto<int>(true, oldCat.StoreId.Value, string.Empty);
                    }
                    else
                    {
                        return new ReturnDto<int>(false, 0, "Not Saved, Error Occurred!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                    return new ReturnDto<int>(false, 0, ex.Message);
                }
            }

            return new ReturnDto<int>(false, 0, "OrderssDto is null");
        }

        public async Task<ReturnDto<ReturnsOrdersDto>> GetReturnsOrderWithItems (HttpContext context, int orderId)  

        {
            try
            {
                // Get the order details, including items
                var order = await _unitOfWork.Orderss.SingleOrDefaultAsync(
                    l => l.Idorders == orderId,
                    o => o.OrderItems, t => t.ReturnsOrder);

                if (order == null)
                    return new ReturnDto<ReturnsOrdersDto>(false, null, "Order not found!");

                // Get existing returns for the order
                var existingReturns = await _unitOfWork.ReturnsOrder
                    .FindAsync(l => l.OrderId == orderId, r => r.ReturnsOrderItem);

                // Map the order to DTO
                var dto = _mapper.Map<ReturnsOrdersDto>(order);

                // Check for existing returns in each order item
                foreach (var item in dto.Itemslist)
                {
                    item.HasExistingReturn = existingReturns.Any(returnOrder =>
                        returnOrder.ReturnsOrderItem.Any(roi => roi.OrderItemsId == item.OrderItemsId));
                }

                return new ReturnDto<ReturnsOrdersDto>(true, dto, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ReturnDto<ReturnsOrdersDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<T>> GetSingleReturnsOrder<T>(HttpContext context, int orderId) where T : class, new()

        {
            try
            {
                // Get the order details, including items
                var order = await _unitOfWork.ReturnsOrder.SingleOrDefaultAsync(
                    l => l.ReturnsOrderId == orderId,
                    o => o.ReturnsOrderItem, o => o.Store, o => o.Order.Payment, t => t.Order, u => u.Order.Address.Governorates, u => u.Order.Address.CityNavigation, u => u.Order.Address.Neighborhood, t => t.Status, t => t.ReturnsOrderStatus, t => t.Order.OrderItems, h => h.InsertedByNavigation, h => h.Order.Address);
                if (order != null)
                {
                    
                    foreach (var item in order.ReturnsOrderItem)
                    {
                        if (item.OrderItems != null)
                        {

                       
                        var product = await _unitOfWork.Product.SingleOrDefaultAsync(p => p.ProductId == item.OrderItems.ProductId);
                          item.OrderItems.Product = product;
                         }
                    }
                }
                if (order == null)
                    return new ReturnDto<T>(false, null, "Order not found!");

                
                // Map the order to DTO
                var dto = _mapper.Map<T>(order);

                
                return new ReturnDto<T>(true, dto, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ReturnDto<T>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<int>> Update (HttpContext context, AcceptReturnsOrdersDto dto, int ReturnsOrderId)  
        {
            if (dto!= null && ReturnsOrderId!=0)
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                // var oldCat = await _unitOfWork.ReturnsOrder.SingleOrDefaultAsync(p => p.ReturnsOrderId == ReturnsOrderId);
                var oldCat = await _unitOfWork.ReturnsOrder.GetAsync(ReturnsOrderId);
                try
                {
                    var newCat = _mapper.Map(dto, oldCat);
                    newCat.UpdateDate = DateTime.UtcNow.AddHours(3); 
                   if(dto.ShippingDate==null)
                    {

                    }
                    if (dto.ShippingReturnBool == null)
                    {
                        dto.ShippingReturnBool = false;
                    }
                    newCat.UpdateBy = stringUserId;
                await _unitOfWork.ReturnsOrder.UpdateAsync(newCat);
                
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        if(oldCat.StoreId!=null)
                        {
                            return new ReturnDto<int>(true, oldCat.StoreId.Value, string.Empty);
                        }
                        else
                            return new ReturnDto<int>(true, 0, string.Empty);
                    }
                    else
                    {
                        return new ReturnDto<int>(false, 0, "Not Saved, Error Occurred!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                    return new ReturnDto<int>(false, 0, ex.Message);
                }
            }

            return new ReturnDto<int>(false, 0, "OrderssDto is null");
        }


        public async Task<ReturnDto<Dictionary<int, int>>> GetCountOrdersByStatuses(HttpContext context, List<int> statuses)
        {
            try
            {
                var orders = await _unitOfWork.ReturnsOrder.FindAsync(o => statuses.Contains(o.StatusId.Value));

                if (orders == null || !orders.Any())
                {
                    return new ReturnDto<Dictionary<int, int>>(false, new Dictionary<int, int>(), "No orders found.");
                }

                var result = orders
                    .GroupBy(o => o.StatusId)
                    .ToDictionary(g => g.Key.Value, g => g.Count());

                return new ReturnDto<Dictionary<int, int>>(true, result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ReturnDto<Dictionary<int, int>>(false, new Dictionary<int, int>(), "An error occurred while processing your request.");
            }
        }
    }
}
