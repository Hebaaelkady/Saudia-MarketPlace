using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs; 
using Kader.DTOs.OrderStatus;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.OrderStatus
{

    public class OrderStatusService : IOrderStatusService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public OrderStatusService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<OrderStatusDto>>> GetOrderStatus(int orderid)
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.OrderStatus> getData = await _unitOfWork.OrderStatus.FindAsync(h => h.IsDeleted == false&&h.OrderId==orderid) ?? new List<Data.DataAccessLayer.Entities.OrderStatus>();
                 
                var map = _mapper.Map<List<OrderStatusDto>>(getData);
                return new ReturnDto<List<OrderStatusDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderStatusDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderStatusDto>>> GetDeletedOrderStatus()
        {

            try
            {
                List<Data.DataAccessLayer.Entities.OrderStatus> getData = await _unitOfWork.OrderStatus.FindAsync(h => h.IsDeleted == true) ?? new List<Data.DataAccessLayer.Entities.OrderStatus>();

                var map = _mapper.Map<List<OrderStatusDto>>(getData);
                return new ReturnDto<List<OrderStatusDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderStatusDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveOrderStatus(HttpContext context, int OrderID, int statusID, int StoreId,string comment)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (OrderID != 0 && statusID!=0)
                {
                    Data.DataAccessLayer.Entities.OrderStatus x = new Data.DataAccessLayer.Entities.OrderStatus();
                    x.OrderId = OrderID;
                    if(StoreId==0)
                    {
                        x.StoreId = null;
                    }
                    else
                    {
                        x.StoreId = StoreId;
                    }
                    x.Comment=comment;              
                    x.StatusId = statusID;
                    x.InsertedBy = stringUserId;
                    await _unitOfWork.OrderStatus.AddAsync(x);
                }
                 

                if (await _unitOfWork.CompleteAsync() > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<List<OrderStatusDto>>> GetLatestOrderStatuses(List<int> StoreId)
        {
            try
            {
                var allOrderStatuses = await _unitOfWork.OrderStatus.FindAsync(l => l.IsDeleted != true && StoreId.Contains(l.StoreId.Value));
                if (allOrderStatuses == null || !allOrderStatuses.Any())
                    return new ReturnDto<List<OrderStatusDto>>(false, null, "Nothing found!");

                // Apply filtering: Check `IsDeleted` and ensure `InsertedBy` is in `userIds`
                var latestStatuses = allOrderStatuses

                    .GroupBy(o => o.OrderId)
                    .Select(g => g.OrderByDescending(o => o.CreatedDate).First())
                    .ToList();


                var mappedData = _mapper.Map<List<OrderStatusDto>>(latestStatuses);
                return new ReturnDto<List<OrderStatusDto>>(true, mappedData, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<List<OrderStatusDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> DeleteOrderStatus(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.OrderStatus.SingleOrDefaultAsync(l => l.OrderStatusId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.OrderStatus.UpdateAsync(newCat);

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
        public async Task<ReturnDto<OrderStatusDto>> GetSingleOrderStatus(int id)
        {
            try
            {
                var getData = await _unitOfWork.OrderStatus.SingleOrDefaultAsync(l => l.IsDeleted == false && l.OrderStatusId == id);
                if (getData == null) return new ReturnDto<OrderStatusDto>(false, null, "Nothing found!");

                var map = _mapper.Map<OrderStatusDto>(getData);
                return new ReturnDto<OrderStatusDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<OrderStatusDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> RestoreDeleteOrderStatus(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.OrderStatus.SingleOrDefaultAsync(l => l.OrderStatusId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.OrderStatus.UpdateAsync(newCat);

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
    }
}
