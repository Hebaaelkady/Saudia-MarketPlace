using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.Ads;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Ads
{

    public interface IAdsService
    {
        Task<ReturnDto<List<AdsDto>>> GetAds();
        Task<ReturnDto<AdsDto>> GetLatestAds();
        Task<ReturnDto<bool>> DeleteAds(HttpContext context, int id);
        Task<ReturnDto<bool>> SaveAds(HttpContext context, AdsDto AdsDto);
        Task<ReturnDto<AdsDto>> GetSingleAds(int id);
        Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles, List<IFormFile> FormFiles1);
    }
}
