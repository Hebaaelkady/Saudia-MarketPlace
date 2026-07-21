using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Address;
using Kader.DTOs.Cities;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Cities
{

    public interface ICitiesService
    {
        Task<ReturnDto<List<CitiesDto>>> GetCities(int id);
        Task<ReturnDto<bool>> SaveAddress(HttpContext context, CitiesDto CitiesDto);
        Task<ReturnDto<bool>> DeleteAddress(HttpContext context, int id);
        Task<ReturnDto<CitiesDto>> GetSingleCity(int id);
    }
}
