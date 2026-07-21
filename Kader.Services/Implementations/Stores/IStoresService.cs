using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Stores;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Stores
{

    public interface IStoresService
    {
        Task<ReturnDto<List<StoresDto>>> GetStores();
        Task<ReturnDto<bool>> SaveStores(HttpContext context, StoresDto StoresDto);
        Task<ReturnDto<bool>> DeleteStores(HttpContext context, int id);
        Task<ReturnDto<StoresDto>> GetSingleStores(int id);
        Task<ReturnDto<List<StoresDto>>> GetDeletedStores();
        Task<ReturnDto<bool>> RestoreDeleteStores(HttpContext context, int id);
    }
}
