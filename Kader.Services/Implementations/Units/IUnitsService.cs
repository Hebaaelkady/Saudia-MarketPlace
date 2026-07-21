using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.Units;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Units
{

    public interface IUnitsService
    {
        Task<ReturnDto<List<UnitsDto>>> GetUnits();
        Task<ReturnDto<bool>> DeleteUnits(HttpContext context, int id);
        Task<ReturnDto<bool>> SaveUnits(HttpContext context, UnitsDto UnitsDto);
        Task<ReturnDto<UnitsDto>> GetSingleUnits(int id);
        Task<ReturnDto<List<UnitsDto>>> GetDeletedUnits();
        Task<ReturnDto<bool>> RestoreDeleteUnits(HttpContext context, int id);
    }
}
