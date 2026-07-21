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
using Kader.DTOs.Pages;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.Pages
{

    public class PagesService : IPagesService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public PagesService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
       
       
        public async Task<ReturnDto<bool>> SavePages(HttpContext context, PagesDto PagesDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                Data.DataAccessLayer.Entities.Pages newCat1 = null;
                if (PagesDto.PageId == 0)
                    {
                     
                        newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Pages>(PagesDto);
                   
                    
                        await _unitOfWork.Pages.AddAsync(newCat1);
                    }
                    else
                    {
                        var oldCat = await _unitOfWork.Pages.GetAsync(PagesDto.PageId);
                         newCat1 = _mapper.Map(PagesDto, oldCat);
                    await _unitOfWork.Pages.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeletePages(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Pages.SingleOrDefaultAsync(l => l.PageId == id);
                
                await _unitOfWork.Pages.RemoveAsync(newCat);

                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
       //for update
        public async Task<ReturnDto<PagesDto>> GetSinglePages(int id)
        {
            try
            {
                var getData = await _unitOfWork.Pages.SingleOrDefaultAsync(l => l.PageId == id );
                if (getData == null) return new ReturnDto<PagesDto>(false, null, "Nothing found!");

                var map = _mapper.Map<PagesDto>(getData);
                return new ReturnDto<PagesDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<PagesDto>(false, null, ex.Message);
            }
        }
      
    }
}
