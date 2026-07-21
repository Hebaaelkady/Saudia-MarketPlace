using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.CatType;
using Kader.DTOs.StoreShippingUsersReport;
using Kader.DTOs.LogPrice;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Kader.Services.Implementations.StoreShippingUsersReport
{

    public class StoreShippingUsersReportService : IStoreShippingUsersReportService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public StoreShippingUsersReportService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<StoreShippingUsersReportDto>>> GetStoreShippingUsersReport(HttpContext context)
        {
            try
            {
                List<Data.DataAccessLayer.Entities.StoreShippingUsersReport> getData = await _unitOfWork.StoreShippingUsersReport.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.StoreShippingUsersReport>();

                var map = _mapper.Map<List<StoreShippingUsersReportDto>>(getData);
                return new ReturnDto<List<StoreShippingUsersReportDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<StoreShippingUsersReportDto>>(false, null, ex.Message);
            }
        }


    }
}
