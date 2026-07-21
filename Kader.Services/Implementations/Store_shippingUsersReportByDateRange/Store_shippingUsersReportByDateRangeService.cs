using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs;
using Kader.DTOs.Store_shippingUsersReportByDateRange;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Store_shippingUsersReportByDateRange
{

    public class Store_shippingUsersReportByDateRangeService : IStore_shippingUsersReportByDateRangeService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public Store_shippingUsersReportByDateRangeService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<Store_shippingUsersReportByDateRangeDto>>> GetAllQuery(DateTime startDate, DateTime endDate)
        {
            try
            {
                var data = await _unitOfWork.Store_shippingUsersReportByDateRange.GetList(
                    startDate != default ? startDate.ToString("yyyy-MM-dd") : "0",
                    endDate != default ? endDate.ToString("yyyy-MM-dd") : "0"
                );

                var mappedData = _mapper.Map<List<Store_shippingUsersReportByDateRangeDto>>(data);

                return new ReturnDto<List<Store_shippingUsersReportByDateRangeDto>>(true, mappedData);
            }
            catch (Exception ex)
            {
                return new ReturnDto<List<Store_shippingUsersReportByDateRangeDto>>(false, null, ex.Message);
            }
        }



    }
}
