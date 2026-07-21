using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.BannerImgs;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.BannerImgs
{

    public interface IBannerImgsService
    {
        Task<ReturnDto<List<BannerImgsDto>>> GetBannerImg();
        Task<ReturnDto<bool>> DeleteBannerImg(HttpContext context, int id);
        Task<ReturnDto<bool>> SaveBannerImg(HttpContext context, BannerImgsDto BannerImgDto);
        Task<ReturnDto<BannerImgsDto>> GetSingleBannerImg(int id);
        Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles);
    }
}
