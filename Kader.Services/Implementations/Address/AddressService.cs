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
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Address
{

    public class AddressService : IAddressService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AddressService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<AddressDto>>> GetAddress(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.Address>? getData = await _unitOfWork.Address.FindAsync(d=>d.IdUser== stringUserId, u => u.CityNavigation, u => u.Neighborhood, u => u.Governorates) ?? new List<Data.DataAccessLayer.Entities.Address>();
                var map = _mapper.Map<List<AddressDto>>(getData);
                return new ReturnDto<List<AddressDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<AddressDto>>(false, null, ex.Message);
            }
        }
       
        public async Task<ReturnDto<bool>> SaveAddress(HttpContext context, AddressDto AddressDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                Data.DataAccessLayer.Entities.Address newCat1 = null;
                if (AddressDto.AddressId == 0|| AddressDto.AddressId == null)
                    {
                         
                    try
                    {
                        
                        newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Address>(AddressDto);
                    }
                    catch (AutoMapperMappingException ex)
                    {
                         
                        Console.WriteLine($"Mapping failed: {ex.Message}");
                        Console.WriteLine($"Error mapping Address to AddressDto: {ex.Message}");
                        Console.WriteLine($"Source: {AddressDto?.ToString()}");
                        Console.WriteLine($"Destination: {AddressDto?.ToString()}");
                    }

                    newCat1.IdUser = stringUserId;
                        await _unitOfWork.Address.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.Address.GetAsync(AddressDto.AddressId);
                         newCat1 = _mapper.Map(AddressDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3); newCat1.IdUser = stringUserId;
                    await _unitOfWork.Address.UpdateAsync(newCat1);
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
                var newCat = await _unitOfWork.Address.SingleOrDefaultAsync(l => l.AddressId == id);
                
                await _unitOfWork.Address.RemoveAsync(newCat);

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
        public async Task<ReturnDto<AddressDto>> GetSingleAddress(int id)
        {
            try
            {
                var getData = await _unitOfWork.Address.SingleOrDefaultAsync(l => l.AddressId == id ,i=>i.CityNavigation);
                if (getData == null) return new ReturnDto<AddressDto>(false, null, "Nothing found!");

                var map = _mapper.Map<AddressDto>(getData);
                return new ReturnDto<AddressDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<AddressDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<AddressDto>> GetActiveAddress(HttpContext context)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var getData = await _unitOfWork.Address.SingleOrDefaultAsync(l => l.IsMain == true&&l.IdUser==stringUserId, p => p.CityNavigation, p => p.Governorates, p => p.Neighborhood);
                if (getData == null) return new ReturnDto<AddressDto>(false, null, "Nothing found!");

                var map = _mapper.Map<AddressDto>(getData);
                return new ReturnDto<AddressDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<AddressDto>(false, null, ex.Message);
            }
        }

    }
}
