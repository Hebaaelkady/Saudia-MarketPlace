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
using Kader.DTOs.Product;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.CatType
{

    public class CatTypeService : ICatTypeService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CatTypeService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<CatTypeDto>>> GetCatogry()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.CatType> getData = await _unitOfWork.CatType.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.CatType>();

                
                var map = _mapper.Map<List<CatTypeDto>>(getData);
                return new ReturnDto<List<CatTypeDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<CatTypeDto>>(false, null, ex.Message);
            }
        }
 

    }
}
