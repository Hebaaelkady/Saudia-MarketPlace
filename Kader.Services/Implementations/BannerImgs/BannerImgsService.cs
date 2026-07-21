using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;  
using Kader.DTOs.BannerImgs;
using Kader.DTOs.ProductImg;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
namespace Kader.Services.Implementations.BannerImgs
{

    public class BannerImgsService : IBannerImgsService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public BannerImgsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<BannerImgsDto>>> GetBannerImg()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.BannerImgs> getData = await _unitOfWork.BannerImgs.FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.BannerImgs>();
                 
                var map = _mapper.Map<List<BannerImgsDto>>(getData);
                return new ReturnDto<List<BannerImgsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<BannerImgsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveBannerImg(HttpContext context, BannerImgsDto BannerImgsDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (BannerImgsDto.BannerImgId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.BannerImgs>(BannerImgsDto);
                    newCat1.InsertedBy = stringUserId;
                    await _unitOfWork.BannerImgs.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.BannerImgs.GetAsync(BannerImgsDto.BannerImgId);
                    var newCat1 = _mapper.Map(BannerImgsDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.BannerImgs.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteBannerImg(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.BannerImgs.SingleOrDefaultAsync(l => l.BannerImgId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.BannerImgs.UpdateAsync(newCat);

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
        public async Task<ReturnDto<BannerImgsDto>> GetSingleBannerImg(int id)
        {
            try
            {
                var getData = await _unitOfWork.BannerImgs.SingleOrDefaultAsync(l => l.IsDeleted == false && l.BannerImgId == id);
                if (getData == null) return new ReturnDto<BannerImgsDto>(false, null, "Nothing found!");

                var map = _mapper.Map<BannerImgsDto>(getData);
                return new ReturnDto<BannerImgsDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<BannerImgsDto>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles)
        {
            List<string> x = new List<string>();
             
                foreach (var file in FormFiles)
                {
                    int maxFileSizeInBytes = 1 * 1024 * 1024; // 1MB

                    if (file != null && file.Length > maxFileSizeInBytes)
                    {
                        // ViewBag.ErrorMessage = "File size exceeds the maximum limit of 1MB.";
                        return new ReturnDto<bool>(false, false, "File size exceeds the maximum limit of 1MB. !");
                    }
                    else
                    {
                        var allowedFileTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();
                        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                        if (!allowedFileTypes.Contains(fileExtension) && !allowedMimeTypes.Contains(file.ContentType))
                        {
                            return new ReturnDto<bool>(false, false, "Invalid file type. Only images (jpg, jpeg, png, gif) are allowed.");
                        }
                        else
                        {
                            var name = Convert.ToString(Guid.NewGuid());
                            var fileName = Path.GetFileName(file.FileName);
                            var newFileName = String.Concat(name, fileExtension);
                            var filepath = "";
                            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newFileName}";

                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                await file.CopyToAsync(fs);
                                fs.Flush();
                            }
                            x.Add(newFileName);
                            var newCat = _mapper.Map<Data.DataAccessLayer.Entities.BannerImgs>(new BannerImgsDto { BannerImgName = newFileName });

                            await _unitOfWork.BannerImgs.AddAsync(newCat);
                        }
                    }
                }
             
            try
            {

                if (_unitOfWork.Complete() > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
            //return x;
        }

        
    }
}
