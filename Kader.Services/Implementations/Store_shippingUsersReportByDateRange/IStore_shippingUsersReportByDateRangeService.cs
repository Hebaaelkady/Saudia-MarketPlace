using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using System;
using Kader.DTOs.Store_shippingUsersReportByDateRange;
namespace Kader.Services.Implementations.Store_shippingUsersReportByDateRange
{

    public interface IStore_shippingUsersReportByDateRangeService
    {
        Task<ReturnDto<List<Store_shippingUsersReportByDateRangeDto>>> GetAllQuery(DateTime startDate, DateTime endDate);
    }
}
