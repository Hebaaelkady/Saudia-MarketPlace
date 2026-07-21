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
using Kader.DTOs.LogPrice;
using Kader.DTOs.ShippingPrice;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.LogPrice
{

    public class UnitsService : ILogPriceService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UnitsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<LogPriceDto>>> GetLogPrice()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.LogPrice> getData = await _unitOfWork.LogPrice.GetAllAsync() ?? new List<Data.DataAccessLayer.Entities.LogPrice>();
                 
                var map = _mapper.Map<List<LogPriceDto>>(getData);
                return new ReturnDto<List<LogPriceDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<LogPriceDto>>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> SaveLogPrice(HttpContext context, LogPriceDto LogPrice)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (LogPrice.LogPriceId == 0)
                {
                        var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.LogPrice>(LogPrice);
                        newCat1.InsertedBy = stringUserId; 
                        await _unitOfWork.LogPrice.AddAsync(newCat1);
                    }                         
                else
                {
                    var oldCat = await _unitOfWork.LogPrice.GetAsync(LogPrice.LogPriceId);
                    var newCat1 = _mapper.Map(LogPrice, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.LogPrice.UpdateAsync(newCat1);
                }
 
                    if (await _unitOfWork.CompleteAsync() > 0)
                        return new ReturnDto<bool>(true, true, string.Empty);
                    else
                        return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

    }
}
