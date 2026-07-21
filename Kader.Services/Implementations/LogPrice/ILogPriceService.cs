using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.LogPrice; 
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.LogPrice
{

    public interface ILogPriceService
    {
        Task<ReturnDto<List<LogPriceDto>>> GetLogPrice();
        Task<ReturnDto<bool>> SaveLogPrice(HttpContext context, LogPriceDto LogPrice);
    }

}
