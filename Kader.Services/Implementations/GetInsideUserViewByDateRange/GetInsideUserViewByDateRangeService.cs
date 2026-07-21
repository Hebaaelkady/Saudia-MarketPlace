using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs;
using Kader.DTOs.GetInsideUserViewByDateRange;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.GetInsideUserViewByDateRange
{

    public class GetInsideUserViewByDateRangeService : IGetInsideUserViewByDateRangeService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public GetInsideUserViewByDateRangeService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<GetInsideUserViewByDateRangeDto>>> GetAllEvalutedByMwghInKaderQuery(DateTime startDate, DateTime endDate)
        {
            try
            {
                var data = await _unitOfWork.GetInsideUserViewByDateRange.GetList(
                    startDate != default ? startDate.ToString("yyyy-MM-dd") : "0",
                    endDate != default ? endDate.ToString("yyyy-MM-dd") : "0"
                );

                var mappedData = _mapper.Map<List<GetInsideUserViewByDateRangeDto>>(data);

                return new ReturnDto<List<GetInsideUserViewByDateRangeDto>>(true, mappedData);
            }
            catch (Exception ex)
            {
                return new ReturnDto<List<GetInsideUserViewByDateRangeDto>>(false, null, ex.Message);
            }
        }



    }
}
