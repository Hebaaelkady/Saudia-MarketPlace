using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.LogQuantity;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.LogQuantity
{

    public interface ILogQuantityService
    {
        Task<ReturnDto<List<LogQuantityDto>>> GetLogQuantity(HttpContext context);
        Task<ReturnDto<bool>> SaveLogQuantity(HttpContext context, LogQuantityDto LogQuantityDto);
        Task<ReturnDto<bool>> DeleteLogQuantity(HttpContext context, int id);
        Task<ReturnDto<LogQuantityDto>> GetSingleLogQuantity(int id); 
    }
}
