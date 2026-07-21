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
using Kader.DTOs.Stores;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Stores
{

    public class StoresService : IStoresService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public StoresService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<StoresDto>>> GetStores()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Stores>? getData = await _unitOfWork.Stores.FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.Stores>();
                var map = _mapper.Map<List<StoresDto>>(getData);
                return new ReturnDto<List<StoresDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<StoresDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<StoresDto>>> GetDeletedStores()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Stores>? getData = await _unitOfWork.Stores.FindAsync(h => h.IsDeleted == true) ?? new List<Data.DataAccessLayer.Entities.Stores>();
                var map = _mapper.Map<List<StoresDto>>(getData);
                return new ReturnDto<List<StoresDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<StoresDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveStores(HttpContext context, StoresDto StoresDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                
                    if (StoresDto.StoreId == 0)
                    {

                        var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Stores>(StoresDto);
                        newCat1.InsertedBy = stringUserId;
                        await _unitOfWork.Stores.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.Stores.GetAsync(StoresDto.StoreId);
                        var newCat1 = _mapper.Map(StoresDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Stores.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteStores(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Stores.SingleOrDefaultAsync(l => l.StoreId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Stores.UpdateAsync(newCat);

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
        public async Task<ReturnDto<bool>> RestoreDeleteStores(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Stores.SingleOrDefaultAsync(l => l.StoreId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Stores.UpdateAsync(newCat);

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

        public async Task<ReturnDto<StoresDto>> GetSingleStores(int id)
        {
            try
            {
                var getData = await _unitOfWork.Stores.SingleOrDefaultAsync(l => l.IsDeleted == false && l.StoreId == id);
                if (getData == null) return new ReturnDto<StoresDto>(false, null, "Nothing found!");

                var map = _mapper.Map<StoresDto>(getData);
                return new ReturnDto<StoresDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<StoresDto>(false, null, ex.Message);
            }
        }

    }
}
