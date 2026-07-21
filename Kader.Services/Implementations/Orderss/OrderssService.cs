using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart; 
using Kader.DTOs.Orderss; 
using Newtonsoft.Json; 
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net.Http; 
using System.Net.Http.Headers;
using System.Text;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Infrastructure.Shared.Implementations; 
using System.Security.Claims; 
namespace Kader.Services.Implementations.Order
{

    public class OrderssService : IOrderssService
    {
        private IUnitOfWork _unitOfWork; private IKeys _keys;
        private IMapper _mapper; private readonly HttpClient _httpClient;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public OrderssService(HttpClient httpClient,IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork; _httpClient = httpClient; _keys = new Keys();
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<T>>> GetOrder<T>(HttpContext context) where T : class, new()
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                List<Data.DataAccessLayer.Entities.Orderss>? getData = await _unitOfWork.Orderss.FindAsync(d=>d.UserId== stringUserId&&(d.StatusId!=11/*|| d.StatusId != null*/),i=>i.Status,k=>k.OrderItems) ?? new List<Data.DataAccessLayer.Entities.Orderss>();
                var map = _mapper.Map<List<T>>(getData);
                return new ReturnDto<List<T>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<T>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<T>>> GetOrderNotMoreThan14Day<T>(HttpContext context) where T : class, new()
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                List<Data.DataAccessLayer.Entities.Orderss>? getData = await _unitOfWork.Orderss.FindAsync(d => d.UserId == stringUserId &&  (d.StatusId != 11 || d.StatusId != null) && (d.StatusId == 5 || d.StatusId == 15) && d.DeliveryDate >= DateTime.UtcNow.AddHours(3).AddDays(-14), i => i.Status, k => k.OrderItems) ?? new List<Data.DataAccessLayer.Entities.Orderss>();
                var map = _mapper.Map<List<T>>(getData);
                return new ReturnDto<List<T>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<T>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> GetOrderNotMoreThan14DayStatus1And3(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                List<Data.DataAccessLayer.Entities.Orderss>? getData = await _unitOfWork.Orderss.FindAsync(d => d.UserId == stringUserId && (d.StatusId != 11 || d.StatusId != null) && ((d.StatusId == 5||d.StatusId == 15) && d.DeliveryDate >= DateTime.UtcNow.AddHours(3).AddDays(-14))||d.StatusId==1 || d.StatusId == 3 || d.StatusId == 4 || d.StatusId == 12 || d.StatusId == 13 || d.StatusId == 14, i => i.Status, k => k.OrderItems) ?? new List<Data.DataAccessLayer.Entities.Orderss>();
                if(getData.Count>0)

                return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(false, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<List<OrderssDto>>> GetDeletedOrder()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Orderss>? getData = await _unitOfWork.Orderss.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.Orderss>();
                var map = _mapper.Map<List<OrderssDto>>(getData);
                return new ReturnDto<List<OrderssDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderssDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveOrderApi(string token, Checkout_frontDto order)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new ReturnDto<bool>(false, false, "Token is null or empty");
            }

            if (order == null)
            {
                return new ReturnDto<bool>(false, false, "Order is null");
            }

            var newCat = _mapper.Map<ApiOrderDto>(order);
            var json = JsonConvert.SerializeObject(newCat);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.PostAsync("https://jinaapi.auditor.sa/SaveOrder", content);

                if (!response.IsSuccessStatusCode)
                {
                    return new ReturnDto<bool>(false, false, $"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReturnDto<bool>>(responseContent);

                if (result != null && result.isSuccess)
                {
                    return result;
                }
                else
                {
                    return new ReturnDto<bool>(false, false, "Failed to save order");
                }
            }
            catch (HttpRequestException ex)
            {
                return new ReturnDto<bool>(false, false, $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ReturnDto<bool>(false, false, $"Unexpected error: {ex.Message}");
            }
        }

        public async Task<ReturnDto<int>> SaveOrder(HttpContext context, SaveCheckoutDto OrderssDto)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (OrderssDto != null)
            {
                Data.DataAccessLayer.Entities.Orderss newCat1 = null;

                 
                    newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Orderss>(OrderssDto);
                    newCat1.UserId = stringUserId;

                    //if (newCat1.OrdersPayment == Guid.Empty)
                    //{
                    //    newCat1.OrdersPayment = Guid.NewGuid(); // Initialize OrderPayment if it's empty
                    //}

                    await _unitOfWork.Orderss.AddAsync(newCat1);
                
                 

                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                         int y = newCat1.Idorders;
                        return new ReturnDto<int>(true,y, string.Empty);
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
        public async Task<ReturnDto<bool>> updateStatus(HttpContext context, int Status,int orderID)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (orderID != 0)
            {
               
                    var oldCat = await _unitOfWork.Orderss.GetAsync(orderID);
           
                oldCat.StatusId = Status;
                    await _unitOfWork.Orderss.UpdateAsync(oldCat);
                 

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
        public async Task<ReturnDto<bool>> update(HttpContext context, AssignStoreOrderDto AssignStoreOrderDto)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            Data.DataAccessLayer.Entities.Orderss newCat1 = null;
            if (AssignStoreOrderDto.Idorders != 0)
            {

                var oldCat = await _unitOfWork.Orderss.GetAsync(AssignStoreOrderDto.Idorders);
                newCat1 = _mapper.Map(AssignStoreOrderDto, oldCat);
                
                await _unitOfWork.Orderss.UpdateAsync(newCat1);
                

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
        public async Task<ReturnDto<bool>> updateSms(HttpContext context, MsgDto AssignStoreOrderDto)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            Data.DataAccessLayer.Entities.Orderss newCat1 = null;
            if (AssignStoreOrderDto.Idorders != 0)
            {

                var oldCat = await _unitOfWork.Orderss.GetAsync(AssignStoreOrderDto.Idorders);
                newCat1 = _mapper.Map(AssignStoreOrderDto, oldCat);

                await _unitOfWork.Orderss.UpdateAsync(newCat1);


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

        public async Task<ReturnDto<bool>> updateStatusWithPayID(  int Status, string PaymentId)
        {
            if (PaymentId != null)
            {
                
                var oldCat = await  _unitOfWork.Orderss.SingleOrDefaultAsync(p=>p.PaymentId== PaymentId);
                oldCat.StatusId = Status;
                await _unitOfWork.Orderss.UpdateAsync(oldCat);
                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    { 
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
        public async Task<ReturnDto<int>> updateshipment(int Idorders, string otoId, DateTime? DeliveryDate, DateTime? ShippingDate, string success,int status,string InsideShippingUser)
        {
            if (Idorders != 0)
            { 
                var oldCat = await _unitOfWork.Orderss.SingleOrDefaultAsync(p => p.Idorders == Idorders);
                oldCat.StatusId = status;
                
                if(InsideShippingUser != null)
                {
                    oldCat.InsideShippingUser = InsideShippingUser;
                }
                if (otoId != null)
                {
                    oldCat.ShippingotoId = otoId;
                }
                if (DeliveryDate != null)
                {
                    oldCat.DeliveryDate = DeliveryDate;
                }
                if (ShippingDate != null)
                {
                    oldCat.ShippingDate = ShippingDate;
                }
                if (success != null)
                {
                    oldCat.ShippingotoMessge = success;
                }
                //oldCat.ShippingDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Orderss.UpdateAsync(oldCat);
                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                    {
                        
                        if(oldCat.StoreId.HasValue)
                        {
                            return new ReturnDto<int>(true, oldCat.StoreId.Value, string.Empty);
                        }
                           else
                        { return new ReturnDto<int>(true, 0, string.Empty); }
                       
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
        public async Task<ReturnDto<bool>> DeleteOrder(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Orderss.SingleOrDefaultAsync(l => l.Idorders == id);
              
                await _unitOfWork.Orderss.UpdateAsync(newCat);

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

       
        public async Task<ReturnDto<T>> GetSingleOrder<T>(HttpContext context, string id) where T : class, new()
        {
            try
            {
                var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
                var Stores = context.User.Claims.Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList();
                var roleClaims = context.User.Claims
                   .Where(c => c.Type == ClaimTypes.Role)
                   .Select(c => c.Value)
                   .ToList();
                Orderss getData = null;

                if (TypeUser == _keys.TypeUser_frontend().ToString())
                {
                    getData = await _unitOfWork.Orderss.SingleOrDefaultAsync(
                        l => l.PaymentId == id &&  (l.StatusId != 11 || l.StatusId != null) && l.UserId == stringUserId,
                        o => o.OrderItems, i2 => i2.OrderStatus, o => o.InsideShippingUserNavigation,
                        s => s.Address, u => u.User, u => u.Address.Governorates, u => u.Address.Neighborhood, f => f.Address.CityNavigation,t=>t.ReturnsOrder
                    );
                }
                else if (isAdmin|| roleClaims.Contains(_keys.ManagOrderRole()))
                {
                    getData = await _unitOfWork.Orderss.SingleOrDefaultAsync(
                        l => l.PaymentId == id, o => o.InsideShippingUserNavigation,  
                        o => o.OrderItems, i2 => i2.OrderStatus, i2 => i2.Store,
                        s => s.Address, s => s.Address, u => u.User, u => u.Address.Governorates, u => u.Address.Neighborhood, f => f.Address.CityNavigation, t => t.ReturnsOrder
                    );
                }
                else/* if (roleClaims.Contains(_keys.storeRole())|| roleClaims.Contains(_keys.ShippingRole()))*/
                {
                    getData = await _unitOfWork.Orderss.SingleOrDefaultAsync(
     l => l.PaymentId == id &&   Stores.Contains(l.StoreId.ToString())
     ,
     o => o.OrderItems, i2 => i2.Store,
     i2 => i2.OrderStatus,
     s => s.Address,
     s => s.Address,
     u => u.User, o => o.InsideShippingUserNavigation,
     u => u.Address.Governorates,
     u => u.Address.Neighborhood,
     f => f.Address.CityNavigation,
     t => t.ReturnsOrder
 );
                }

                if (getData != null)
                {
                    foreach (var item in getData.OrderItems)
                    {
                        var product = await _unitOfWork.Product.SingleOrDefaultAsync(p => p.ProductId == item.ProductId, i2 => i2.UnitNavigation);
                        var color = await _unitOfWork.Colors.SingleOrDefaultAsync(p => p.ColorId == item.ColorId);
                        var ReturnsOrderItem = await _unitOfWork.ReturnsOrderItem.SingleOrDefaultAsync(p => p.OrderItemsId == item.OrderItemsId);
                        item.Product = product;
                        item.Color = color;
                        item.ReturnsOrderItem = ReturnsOrderItem;

                    }
                }

                if (getData == null)
                    return new ReturnDto<T>(false, null, "Nothing found!");

                var map = _mapper.Map<T>(getData);
                return new ReturnDto<T>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<T>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> CheckCode(HttpContext context, string id)  
        {
            try
            {
                Orderss getData = getData = await _unitOfWork.Orderss.SingleOrDefaultAsync( l => l.OrdersNo == id);
                if (getData == null)
                    return new ReturnDto<bool>(false, false, "Nothing found!");
                return new ReturnDto<bool>(true, true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderssDto>>> GetAllOrderNotSendMsgToUser(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                // Ensure the data retrieval returns a list of Orderss entities
                List<Orderss> getData = await _unitOfWork.Orderss.FindAsync(
                    o => o.MsgCofeToUser != true && (o.StatusId == 14)
                    
                ) ?? new List<Orderss>();

                // Check if getData is of the expected type
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<OrderssDto>>(false, null, "No orders found.");
                }

                // Map the list of Orderss entities to a list of OrderBackendDto
                var map = _mapper.Map<List<OrderssDto>>(getData);

                return new ReturnDto<List<OrderssDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderssDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderssDto>>> GetAllOrderNotSendMsgToDelivery(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                // Ensure the data retrieval returns a list of Orderss entities
                List<Orderss> getData = await _unitOfWork.Orderss.FindAsync(
                    o => o.MsgToDelivery != true && (o.StatusId == 14)

                ) ?? new List<Orderss>();

                // Check if getData is of the expected type
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<OrderssDto>>(false, null, "No orders found.");
                }

                // Map the list of Orderss entities to a list of OrderBackendDto
                var map = _mapper.Map<List<OrderssDto>>(getData);

                return new ReturnDto<List<OrderssDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderssDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<List<OrderBackendDto>>> GetAllOrder(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                // Ensure the data retrieval returns a list of Orderss entities
                List<Orderss> getData = await _unitOfWork.Orderss.FindAsync(
                    o => o.AddressId != null && (o.StatusId != 11 || o.StatusId != null),
                    i1 => i1.Address, 
                    i2 => i2.OrderItems,
                    i3 => i3.User,
                    i4 => i4.Status
                ) ?? new List<Orderss>();

                // Check if getData is of the expected type
                if (getData == null || !getData.Any())
                {
                    return new ReturnDto<List<OrderBackendDto>>(false, null, "No orders found.");
                }

                // Map the list of Orderss entities to a list of OrderBackendDto
                var map = _mapper.Map<List<OrderBackendDto>>(getData);

                return new ReturnDto<List<OrderBackendDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderBackendDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderBackendDto>>> GetAllOrderByStatus(HttpContext context, int status)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                var roleClaims = context.User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                var typeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;
                var Stores = context.User.Claims
                    .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value))
                    .Select(c => c.Value)
                    .ToList();
                var StoresShipping = context.User.Claims
                    .Where(c => c.Type == "StoresShipping" && !string.IsNullOrEmpty(c.Value))
                    .Select(c => c.Value)
                    .ToList();

                List<Orderss> orders;

                if (roleClaims.Any())
                {
                    if (roleClaims.Contains(_keys.storeRole()) || roleClaims.Contains(_keys.ShippingRole()))
                    {
                        // Query for users with restricted access (Store or Shipping Role)
                        orders = await _unitOfWork.Orderss.FindAsync(
                            o => o.AddressId != null &&
                                 Stores.Any(id => id == o.StoreId.ToString()) &&
                                 o.StatusId == status &&
                                 (o.StatusId != 11 || o.StatusId != null),
                            i1 => i1.Address, i1 => i1.Payment,
                            i2 => i2.OrderItems,
                            i3 => i3.User,
                            i4 => i4.Status
                        ) ?? new List<Orderss>();
                    }
                    else
                    {
                        // Query for Admin or other roles
                        orders = await _unitOfWork.Orderss.FindAsync(
                            o => o.AddressId != null &&
                                 o.StatusId == status &&
                                 (o.StatusId != 11 || o.StatusId != null),
                            i1 => i1.Address, i1 => i1.Payment,
                            i2 => i2.OrderItems,
                            i3 => i3.User,
                            i4 => i4.Status
                        ) ?? new List<Orderss>();
                    }
                }
                else
                {
                    return new ReturnDto<List<OrderBackendDto>>(false, null, "User has no roles assigned.");
                }

                if (orders == null || !orders.Any())
                {
                    return new ReturnDto<List<OrderBackendDto>>(false, null, "No orders found.");
                }

                // ✅ Map orders to DTOs
                var mappedOrders = _mapper.Map<List<OrderBackendDto>>(orders);

                // ✅ Assign OrderSource (Store / ShippingStore / General)
                foreach (var orderDto in mappedOrders)
                {
                    if (Stores.Contains(orderDto.StoreId.ToString()))
                    {
                        orderDto.OrderSource = "Store";  // Store Order
                    }
                    else if (StoresShipping.Contains(orderDto.StoreId.ToString()))
                    {
                        orderDto.OrderSource = "ShippingStore";  // Shipping Order
                    }
                    else
                    {
                        orderDto.OrderSource = "General";  // Admin/Other Orders
                    }
                }

                return new ReturnDto<List<OrderBackendDto>>(true, mappedOrders, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderBackendDto>>(false, null, "An error occurred while processing your request.");
            }
        }

         public async Task<ReturnDto<Dictionary<int, int>>> GetCountOrdersByStatuses(HttpContext context, List<int> statuses)
        {
            try
            {
                var orders = await _unitOfWork.Orderss.FindAsync(o => statuses.Contains(o.StatusId.Value));

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
