using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.CatType
{

    public interface ICatTypeService
    {
        Task<ReturnDto<List<CatTypeDto>>> GetCatogry();
    }
}
