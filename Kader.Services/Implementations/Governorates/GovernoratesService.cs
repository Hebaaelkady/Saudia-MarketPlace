using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs; 
using Kader.DTOs.Governorates;
using Kader.DTOs.Product; 
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Kader.Services.Implementations.Governorates
{

    public class GovernoratesService : IGovernoratesService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public GovernoratesService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<GovernoratesDto>>> GetGovernorates(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.Governorates>? getData = await _unitOfWork.Governorates.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.Governorates>();
                var map = _mapper.Map<List<GovernoratesDto>>(getData);
                return new ReturnDto<List<GovernoratesDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<GovernoratesDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> SaveGovernorates(HttpContext context, GovernoratesDto GovernoratesDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (GovernoratesDto.GovernorateId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Governorates>(GovernoratesDto);
                    newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.Governorates.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.Governorates.GetAsync(GovernoratesDto.GovernorateId);
                    var newCat1 = _mapper.Map(GovernoratesDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Governorates.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteGovernorates(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Governorates.SingleOrDefaultAsync(l => l.GovernorateId == id);

                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Governorates.UpdateAsync(newCat);

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
