using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Colors;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Colors
{

    public interface IColorsService
    {
        Task<ReturnDto<List<ColorsDto>>> GetColors();
        Task<ReturnDto<bool>> SaveColors(HttpContext context, ColorsDto ColorsDto);
        Task<ReturnDto<bool>> DeleteColors(HttpContext context, int id);
        Task<ReturnDto<ColorsDto>> GetSingleColors(int id);
        Task<ReturnDto<List<ColorsDto>>> GetDeletedColors();
        Task<ReturnDto<bool>> RestoreDeleteColors(HttpContext context, int id);
    }
}
