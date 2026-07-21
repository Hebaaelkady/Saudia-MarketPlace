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
using Kader.DTOs.LogPrice;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.RoleDetail
{

    public class RoleDetailService : IRoleDetailService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RoleDetailService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<RoleDetailDto>>> GetRoleDetail()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.RoleDetail
                    > getData = await _unitOfWork.RoleDetail.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.RoleDetail>();
                 
                var map = _mapper.Map<List<RoleDetailDto>>(getData);
                return new ReturnDto<List<RoleDetailDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<RoleDetailDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> SaveRoleDetail(HttpContext context, IList<RoleDetailDto> RoleDetailDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                //    
                foreach (var RoleDetailDtoDto in RoleDetailDto)
                {
                    if (RoleDetailDtoDto.RoleDetaailId == 0)
                    {
                        var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.RoleDetail>(RoleDetailDtoDto);
                        newCat1.CreatedBy = stringUserId;
                        await _unitOfWork.RoleDetail.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.RoleDetail.GetAsync(RoleDetailDtoDto.RoleDetaailId);
                        var newCat1 = _mapper.Map(RoleDetailDtoDto, oldCat);
                        newCat1.LastModifiedOn = DateTime.UtcNow.AddHours(3);
                        newCat1.LastModifiedBy1 = stringUserId; /*newCat1.ProductId = ProductID;*/
                        await _unitOfWork.RoleDetail.UpdateAsync(newCat1);
                    }
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

        public async Task<ReturnDto<bool>> DeleteRoleDetail(HttpContext context, string id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.RoleDetail.SingleOrDefaultAsync(l => l.RoleId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedOn = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.RoleDetail.UpdateAsync(newCat);

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
