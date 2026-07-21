using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.StoreShippingUsersReport;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.StoreShippingUsersReport
{

    public interface IStoreShippingUsersReportService
    {
        Task<ReturnDto<List<StoreShippingUsersReportDto>>> GetStoreShippingUsersReport(HttpContext context);

    }
}
