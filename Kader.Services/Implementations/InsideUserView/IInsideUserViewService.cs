using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.InsideUserView;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.InsideUserView
{

    public interface IInsideUserViewService
    {
        Task<ReturnDto<List<InsideUserViewDto>>> GetInsideUserView(HttpContext context);
        
    }
}
