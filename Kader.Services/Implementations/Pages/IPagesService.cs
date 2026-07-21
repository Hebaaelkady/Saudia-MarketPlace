using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Pages;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Pages
{

    public interface IPagesService
    {
        Task<ReturnDto<bool>> SavePages(HttpContext context, PagesDto PagesDto);
        Task<ReturnDto<bool>> DeletePages(HttpContext context, int id);
        Task<ReturnDto<PagesDto>> GetSinglePages(int id);
    }
}
