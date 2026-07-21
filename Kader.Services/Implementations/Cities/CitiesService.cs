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
using Kader.DTOs.Cities;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Implementations.Cities;
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Kader.Services.Implementations.Address
{

    public class CitiesService : ICitiesService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CitiesService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<CitiesDto>>> GetCities(int id)
        {
            try
            { 
                List<Data.DataAccessLayer.Entities.Cities>? getData = await _unitOfWork.Cities.FindAsync(d => d.GovernorateId == id) ?? new List<Data.DataAccessLayer.Entities.Cities>();
                var map = _mapper.Map<List<CitiesDto>>(getData);
                return new ReturnDto<List<CitiesDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<CitiesDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> SaveAddress(HttpContext context, CitiesDto CitiesDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (CitiesDto.CityId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Cities>(CitiesDto);
                    newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.Cities.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.Cities.GetAsync(CitiesDto.CityId);
                    var newCat1 = _mapper.Map(CitiesDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Cities.UpdateAsync(newCat1);
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
                var newCat = await _unitOfWork.Cities.SingleOrDefaultAsync(l => l.CityId == id);

                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Cities.UpdateAsync(newCat);

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

        public async Task<ReturnDto<CitiesDto>> GetSingleCity(int id)
        {
            try
            {
                var getData = await _unitOfWork.Cities.SingleOrDefaultAsync(l => l.CityId == id);
                if (getData == null) return new ReturnDto<CitiesDto>(false, null, "Nothing found!");

                var map = _mapper.Map<CitiesDto>(getData);
                return new ReturnDto<CitiesDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<CitiesDto>(false, null, ex.Message);
            }
        }

       
    }
}
