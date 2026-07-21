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
using Kader.DTOs.InsideUserView;
using Kader.DTOs.LogPrice;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.InsideUserView
{

    public class InsideUserViewService : IInsideUserViewService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public InsideUserViewService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<InsideUserViewDto>>> GetInsideUserView(HttpContext context)
        {
            try
            {
                List<Data.DataAccessLayer.Entities.InsideUserView> getData = await _unitOfWork.InsideUserView.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.InsideUserView>();

                var map = _mapper.Map<List<InsideUserViewDto>>(getData);
                return new ReturnDto<List<InsideUserViewDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<InsideUserViewDto>>(false, null, ex.Message);
            }
        }
        
       
    }
}
