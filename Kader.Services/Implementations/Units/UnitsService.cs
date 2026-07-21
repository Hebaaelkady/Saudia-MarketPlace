using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs; 
using Kader.DTOs.Units;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Units
{

    public class UnitsService : IUnitsService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UnitsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<UnitsDto>>> GetUnits()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.Units> getData = await _unitOfWork.Units.FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.Units>();
                 
                var map = _mapper.Map<List<UnitsDto>>(getData);
                return new ReturnDto<List<UnitsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<UnitsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<UnitsDto>>> GetDeletedUnits()
        {

            try
            {
                List<Data.DataAccessLayer.Entities.Units> getData = await _unitOfWork.Units.FindAsync(h => h.IsDeleted == true) ?? new List<Data.DataAccessLayer.Entities.Units>();

                var map = _mapper.Map<List<UnitsDto>>(getData);
                return new ReturnDto<List<UnitsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<UnitsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveUnits(HttpContext context, UnitsDto UnitsDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (UnitsDto.UnitId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Units>(UnitsDto);
                    newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.Units.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.Units.GetAsync(UnitsDto.UnitId);
                    var newCat1 = _mapper.Map(UnitsDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Units.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteUnits(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Units.SingleOrDefaultAsync(l => l.UnitId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Units.UpdateAsync(newCat);

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
        public async Task<ReturnDto<UnitsDto>> GetSingleUnits(int id)
        {
            try
            {
                var getData = await _unitOfWork.Units.SingleOrDefaultAsync(l => l.IsDeleted == false && l.UnitId == id);
                if (getData == null) return new ReturnDto<UnitsDto>(false, null, "Nothing found!");

                var map = _mapper.Map<UnitsDto>(getData);
                return new ReturnDto<UnitsDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<UnitsDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> RestoreDeleteUnits(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Units.SingleOrDefaultAsync(l => l.UnitId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Units.UpdateAsync(newCat);

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
