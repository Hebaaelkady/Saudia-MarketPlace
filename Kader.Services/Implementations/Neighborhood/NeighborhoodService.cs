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
using Kader.DTOs.Neighborhood;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Implementations.Neighborhood;
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Kader.Services.Implementations.Address
{

    public class NeighborhoodService : INeighborhoodService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public NeighborhoodService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<NeighborhoodDto>>> GetNeighborhood(int id)
        {
            try
            { 
                List<Data.DataAccessLayer.Entities.Neighborhood>? getData = await _unitOfWork.Neighborhood.FindAsync(d => d.CityId == id) ?? new List<Data.DataAccessLayer.Entities.Neighborhood>();
                var map = _mapper.Map<List<NeighborhoodDto>>(getData);
                return new ReturnDto<List<NeighborhoodDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<NeighborhoodDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> SaveAddress(HttpContext context, NeighborhoodDto NeighborhoodDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (NeighborhoodDto.CityId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Neighborhood>(NeighborhoodDto);
                    
                    await _unitOfWork.Neighborhood.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.Neighborhood.GetAsync(NeighborhoodDto.NeighborhoodId);
                    var newCat1 = _mapper.Map(NeighborhoodDto, oldCat);
                 
                    await _unitOfWork.Neighborhood.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteAddress(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Neighborhood.SingleOrDefaultAsync(l => l.CityId == id);
                 
                await _unitOfWork.Neighborhood.UpdateAsync(newCat);

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

        public async Task<ReturnDto<NeighborhoodDto>> GetSingleCity(int id)
        {
            try
            {
                var getData = await _unitOfWork.Neighborhood.SingleOrDefaultAsync(l => l.CityId == id);
                if (getData == null) return new ReturnDto<NeighborhoodDto>(false, null, "Nothing found!");

                var map = _mapper.Map<NeighborhoodDto>(getData);
                return new ReturnDto<NeighborhoodDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<NeighborhoodDto>(false, null, ex.Message);
            }
        }

       
    }
}
