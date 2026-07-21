using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Address;
using Kader.DTOs.Governorates;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Governorates
{

    public interface IGovernoratesService
    {
        Task<ReturnDto<List<GovernoratesDto>>> GetGovernorates(HttpContext context);
        Task<ReturnDto<bool>> SaveGovernorates(HttpContext context, GovernoratesDto GovernoratesDto);
        Task<ReturnDto<bool>> DeleteGovernorates(HttpContext context, int id);
     
    }
}
