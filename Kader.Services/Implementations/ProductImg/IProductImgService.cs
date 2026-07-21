using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.ProductImg;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.ProductImg
{

    public interface IProductImgService
    {
        Task<ReturnDto<List<ProductImgDto>>> GetProductImg(HttpContext context, int ProductId);
        Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles, int ProductId);
        string AddFile(IFormFile file, string name);
        Task<ReturnDto<bool>> DeleteFile(HttpContext context, int ProductImgId);
        Task<ReturnDto<bool>> DeleteAllFile(HttpContext context, int id);
    }
}
