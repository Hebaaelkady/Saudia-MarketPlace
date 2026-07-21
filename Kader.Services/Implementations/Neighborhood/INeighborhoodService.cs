using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Address;
using Kader.DTOs.Neighborhood;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Neighborhood
{

    public interface INeighborhoodService
    {
        Task<ReturnDto<List<NeighborhoodDto>>> GetNeighborhood(int id);
        Task<ReturnDto<bool>> SaveAddress(HttpContext context, NeighborhoodDto NeighborhoodDto);
        Task<ReturnDto<bool>> DeleteAddress(HttpContext context, int id);
        Task<ReturnDto<NeighborhoodDto>> GetSingleCity(int id);
    }
}
