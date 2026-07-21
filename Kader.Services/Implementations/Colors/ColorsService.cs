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
using Kader.DTOs.Colors;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Colors
{

    public class ColorsService : IColorsService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ColorsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<ColorsDto>>> GetColors()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Colors>? getData = await _unitOfWork.Colors.FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.Colors>();
                var map = _mapper.Map<List<ColorsDto>>(getData);
                return new ReturnDto<List<ColorsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ColorsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<ColorsDto>>> GetDeletedColors()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.Colors>? getData = await _unitOfWork.Colors.FindAsync(h => h.IsDeleted == true) ?? new List<Data.DataAccessLayer.Entities.Colors>();
                var map = _mapper.Map<List<ColorsDto>>(getData);
                return new ReturnDto<List<ColorsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ColorsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveColors(HttpContext context, ColorsDto ColorsDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                
                    if (ColorsDto.ColorId == 0)
                    {
                        var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Colors>(ColorsDto);
                        newCat1.InsertedBy = stringUserId;
                        await _unitOfWork.Colors.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.Colors.GetAsync(ColorsDto.ColorId);
                        var newCat1 = _mapper.Map(ColorsDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Colors.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteColors(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Colors.SingleOrDefaultAsync(l => l.ColorId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Colors.UpdateAsync(newCat);

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
        public async Task<ReturnDto<bool>> RestoreDeleteColors(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Colors.SingleOrDefaultAsync(l => l.ColorId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Colors.UpdateAsync(newCat);

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

        public async Task<ReturnDto<ColorsDto>> GetSingleColors(int id)
        {
            try
            {
                var getData = await _unitOfWork.Colors.SingleOrDefaultAsync(l => l.IsDeleted == false && l.ColorId == id);
                if (getData == null) return new ReturnDto<ColorsDto>(false, null, "Nothing found!");

                var map = _mapper.Map<ColorsDto>(getData);
                return new ReturnDto<ColorsDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<ColorsDto>(false, null, ex.Message);
            }
        }

    }
}
