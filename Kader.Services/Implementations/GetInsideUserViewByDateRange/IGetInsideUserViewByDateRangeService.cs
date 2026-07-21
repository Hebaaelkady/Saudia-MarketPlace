using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using System;
using Kader.DTOs.GetInsideUserViewByDateRange;
namespace Kader.Services.Implementations.GetInsideUserViewByDateRange
{

    public interface IGetInsideUserViewByDateRangeService
    {
        Task<ReturnDto<List<GetInsideUserViewByDateRangeDto>>> GetAllEvalutedByMwghInKaderQuery(DateTime startDate, DateTime endDate);
    }
}
