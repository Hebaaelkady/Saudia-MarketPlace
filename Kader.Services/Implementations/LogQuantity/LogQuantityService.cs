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
using Kader.DTOs.LogQuantity;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.LogQuantity
{

    public class LogQuantityService : ILogQuantityService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public LogQuantityService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<LogQuantityDto>>> GetLogQuantity(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.LogQuantity>? getData = await _unitOfWork.LogQuantity.FindAsync(d=>d.IsDeleted== false, u => u.Product) ?? new List<Data.DataAccessLayer.Entities.LogQuantity>();
                var map = _mapper.Map<List<LogQuantityDto>>(getData);
                return new ReturnDto<List<LogQuantityDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<LogQuantityDto>>(false, null, ex.Message);
            }
        }
       
        public async Task<ReturnDto<bool>> SaveLogQuantity(HttpContext context, LogQuantityDto LogQuantityDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                Data.DataAccessLayer.Entities.LogQuantity newCat1 = null;
                if (LogQuantityDto.LogQuantityId == 0)
                    {
                         
                   
                    newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.LogQuantity>(LogQuantityDto);
                    newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.LogQuantity.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.LogQuantity.GetAsync(LogQuantityDto.LogQuantityId);
                         newCat1 = _mapper.Map(LogQuantityDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3); newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.LogQuantity.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteLogQuantity(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.LogQuantity.SingleOrDefaultAsync(l => l.LogQuantityId == id);
                
                await _unitOfWork.LogQuantity.RemoveAsync(newCat);

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
        public async Task<ReturnDto<LogQuantityDto>> GetSingleLogQuantity(int id)
        {
            try
            {
                var getData = await _unitOfWork.LogQuantity.SingleOrDefaultAsync(l => l.LogQuantityId == id ,i=>i.Product);
                if (getData == null) return new ReturnDto<LogQuantityDto>(false, null, "Nothing found!");

                var map = _mapper.Map<LogQuantityDto>(getData);
                return new ReturnDto<LogQuantityDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<LogQuantityDto>(false, null, ex.Message);
            }
        }
       
    }
}
