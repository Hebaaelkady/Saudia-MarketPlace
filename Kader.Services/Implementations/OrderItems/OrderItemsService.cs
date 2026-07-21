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
using Kader.DTOs.OrderItems;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.DTOs.Cart;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.OrderItems
{

    public class OrderItemsService : IOrderItemsService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public OrderItemsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
     

    public async Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.OrderItems>? getData = await _unitOfWork.OrderItems.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.OrderItems>();
                var map = _mapper.Map<List<OrderItemsDto>>(getData);
                return new ReturnDto<List<OrderItemsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderItemsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems(int typeGomlaOrQt3a)
        {
            try
            {
                var getData = await _unitOfWork.OrderItems.FindAsync(l => l.TypeGomlaOrQt3 == typeGomlaOrQt3a);
                if (getData == null) return new ReturnDto<List<OrderItemsDto>>(false, null, "Nothing found!");

                var map = _mapper.Map<List<OrderItemsDto>>(getData);
                return new ReturnDto<List<OrderItemsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderItemsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderItemsDto>>> GetDeletedOrderItems()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.OrderItems>? getData = await _unitOfWork.OrderItems.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.OrderItems>();
                var map = _mapper.Map<List<OrderItemsDto>>(getData);
                return new ReturnDto<List<OrderItemsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderItemsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveOrderItems(HttpContext context, IList<Itemslist> OrderItems, int OrderId)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                 
                if (OrderItems != null)
                {
                    foreach (var OrderItemsDto in OrderItems)
                    {
                        try { 
                            var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.OrderItems>(OrderItemsDto);
                            newCat1.InsertBy = stringUserId;
                            newCat1.OrderId = OrderId;
                            await _unitOfWork.OrderItems.AddAsync(newCat1);
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
                else
                {
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> DeleteOrderItems(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.OrderItems.SingleOrDefaultAsync(l => l.ColorId == id);
               
                await _unitOfWork.OrderItems.UpdateAsync(newCat);

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
        public async Task<ReturnDto<bool>> RestoreDeleteOrderItems(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.OrderItems.SingleOrDefaultAsync(l => l.ColorId == id);
                
                await _unitOfWork.OrderItems.UpdateAsync(newCat);

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

        public async Task<ReturnDto<List<OrderItemsDto>>> GetOrderItems(string id)
        {
            try
            {
                var getData = await _unitOfWork.OrderItems.FindAsync(l =>   l.Order.PaymentId == id);
                if (getData == null) return new ReturnDto<List<OrderItemsDto>> (false, null, "Nothing found!");

                var map = _mapper.Map<List<OrderItemsDto>> (getData);
                return new ReturnDto<List<OrderItemsDto>> (true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderItemsDto>>(false, null, ex.Message);
            }
        }

        
    }
}
