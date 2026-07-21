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
using Kader.DTOs.ReturnsOrderItem;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.ReturnsOrderItem
{

    public class ReturnsOrderItemService : IReturnsOrderItemService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ReturnsOrderItemService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        //public async Task<ReturnDto<List<ReturnsOrderItemDto>>> GetReturnsOrderItem(HttpContext context)
        //{
        //    try
        //    {
        //        var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        //        List<Data.DataAccessLayer.Entities.ReturnsOrderItem>? getData = await _unitOfWork.ReturnsOrderItem.FindAsync(d=>d.IdUser== stringUserId, u => u.CityNavigation, u => u.Neighborhood, u => u.Governorates) ?? new List<Data.DataAccessLayer.Entities.ReturnsOrderItem>();
        //        var map = _mapper.Map<List<ReturnsOrderItemDto>>(getData);
        //        return new ReturnDto<List<ReturnsOrderItemDto>>(true, map, string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Kader.System.Error: {ex.Message}");
        //        return new ReturnDto<List<ReturnsOrderItemDto>>(false, null, ex.Message);
        //    }
        //}

        public async Task<ReturnDto<bool>> SaveReturnsOrder(HttpContext context, IList<ReturnsOrderItemDto> OrderItems, int OrderId)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (OrderItems != null)
                {
                    foreach (var OrderItemsDto in OrderItems)
                    {
                        try
                        {
                            var existingItem = await _unitOfWork.ReturnsOrderItem
                        .FindAsync(roi => roi.OrderItemsId == OrderItemsDto.OrderItemsId); // Use FirstOrDefaultAsync instead of FindAsync

                            if (existingItem != null)
                            {
                                // If the item exists, skip it to avoid duplicate insertion
                                continue;
                            }

                            var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.ReturnsOrderItem>(OrderItemsDto);
                            newCat1.InsertedBy = stringUserId;
                            newCat1.ReturnsOrderId = OrderId;
                            await _unitOfWork.ReturnsOrderItem.AddAsync(newCat1);
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
        public async Task<ReturnDto<List<IncreaseReturnsOrderItemDto>>> GetReturnsOrderItems(int id)
        {
            try
            {
                var getData = await _unitOfWork.ReturnsOrderItem.FindAsync(l => l.ReturnsOrderId == id, r => r.OrderItems, r => r.OrderItems.Product);
                if (getData == null) return new ReturnDto<List<IncreaseReturnsOrderItemDto>>(false, null, "Nothing found!");

                var map = _mapper.Map<List<IncreaseReturnsOrderItemDto>>(getData);
                return new ReturnDto<List<IncreaseReturnsOrderItemDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<List<IncreaseReturnsOrderItemDto>>(false, null, ex.Message);
            }
        }


    }
}
