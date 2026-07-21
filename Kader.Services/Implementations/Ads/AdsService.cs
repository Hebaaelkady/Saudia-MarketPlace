using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;  
using Kader.DTOs.Ads;
using Kader.DTOs.ProductImg;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
namespace Kader.Services.Implementations.Ads
{

    public class AdsService : IAdsService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AdsService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        public async Task<ReturnDto<AdsDto>> GetLatestAds()
        {
            try
            {
                var getData = await _unitOfWork.Ads
                    .FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.Ads>();

                if (!getData.Any())
                    return new ReturnDto<AdsDto>(false, null, "No ads found.");

                // Get the latest ad where at least one of AdName1 or AdName2 is not null
                var lastAd = getData.OrderByDescending(a => a.AdsId)
                                    .FirstOrDefault(a => !string.IsNullOrEmpty(a.AdName1) || !string.IsNullOrEmpty(a.AdName2));

                if (lastAd == null)
                    return new ReturnDto<AdsDto>(false, null, "No valid ads found.");

                // Ensure AdName1 has a value, if null, get the last available AdName1
                if (string.IsNullOrEmpty(lastAd.AdName1))
                {
                    lastAd.AdName1 = getData.Where(a => !string.IsNullOrEmpty(a.AdName1))
                                            .OrderByDescending(a => a.AdsId)
                                            .Select(a => a.AdName1)
                                            .FirstOrDefault();
                }

                // Ensure AdName2 has a value, if null, get the last available AdName2
                if (string.IsNullOrEmpty(lastAd.AdName2))
                {
                    lastAd.AdName2 = getData.Where(a => !string.IsNullOrEmpty(a.AdName2))
                                            .OrderByDescending(a => a.AdsId)
                                            .Select(a => a.AdName2)
                                            .FirstOrDefault();
                }

                // If still null, fallback to any available AdName1 or AdName2
                if (string.IsNullOrEmpty(lastAd.AdName1) && string.IsNullOrEmpty(lastAd.AdName2))
                {
                    lastAd.AdName1 = getData.Select(a => a.AdName1).FirstOrDefault(a => !string.IsNullOrEmpty(a));
                    lastAd.AdName2 = getData.Select(a => a.AdName2).FirstOrDefault(a => !string.IsNullOrEmpty(a));
                }

                var map = _mapper.Map<AdsDto>(lastAd);
                return new ReturnDto<AdsDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<AdsDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<AdsDto>>> GetAds()
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.Ads> getData = await _unitOfWork.Ads.FindAsync(h => h.IsDeleted == false ) ?? new List<Data.DataAccessLayer.Entities.Ads>();
                 
                var map = _mapper.Map<List<AdsDto>>(getData);
                return new ReturnDto<List<AdsDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<AdsDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveAds(HttpContext context, AdsDto AdsDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (AdsDto.AdsId == 0)
                {
                    var newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Ads>(AdsDto);
                    newCat1.InsertedBy = stringUserId;
                    newCat1.IsDeleted = false;
                    await _unitOfWork.Ads.AddAsync(newCat1);
                }
                else
                {
                    var oldCat = await _unitOfWork.Ads.GetAsync(AdsDto.AdsId);
                    var newCat1 = _mapper.Map(AdsDto, oldCat);
                    newCat1.UpdateDate = DateTime.UtcNow.AddHours(3);
                    newCat1.UpdateBy = stringUserId; /*newCat1.ProductId = ProductID;*/
                    await _unitOfWork.Ads.UpdateAsync(newCat1);
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

        public async Task<ReturnDto<bool>> DeleteAds(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Ads.SingleOrDefaultAsync(l => l.AdsId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Ads.UpdateAsync(newCat);

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
        public async Task<ReturnDto<AdsDto>> GetSingleAds(int id)
        {
            try
            {
                var getData = await _unitOfWork.Ads.SingleOrDefaultAsync(l => l.IsDeleted == false && l.AdsId == id);
                if (getData == null) return new ReturnDto<AdsDto>(false, null, "Nothing found!");

                var map = _mapper.Map<AdsDto>(getData);
                return new ReturnDto<AdsDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<AdsDto>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles, List<IFormFile> FormFiles1)
        {
            int maxFileSizeInBytes = 1 * 1024 * 1024; // 1MB
            var allowedFileTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(uploadPath); // Ensure directory exists

            string adName1 = null;
            string adName2 = null;

            async Task<string> SaveFile(IFormFile file)
            {
                if (file == null || file.Length == 0 || file.Length > maxFileSizeInBytes)
                    return null;

                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedFileTypes.Contains(fileExtension) || !allowedMimeTypes.Contains(file.ContentType))
                    return null;

                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, newFileName);

                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }
                    return newFileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File Save Error: {ex.Message}");
                    return null;
                }
            }

            // Process FormFiles (first set of files)
            if (FormFiles != null && FormFiles.Count > 0)
            {
                adName1 = await SaveFile(FormFiles.First());
            }

            // Process FormFiles1 (second set of files)
            if (FormFiles1 != null && FormFiles1.Count > 0)
            {
                adName2 = await SaveFile(FormFiles1.First());
            }

            // Ensure at least one file was saved before inserting into the database
            if (adName1 != null || adName2 != null)
            {
                var newAd = _mapper.Map<Data.DataAccessLayer.Entities.Ads>(
                    new AdsDto
                    {
                        AdName1 = adName1,
                        AdName2 = adName2
                    });
                newAd.IsDeleted = false;
                await _unitOfWork.Ads.AddAsync(newAd);

                try
                {
                    if (await _unitOfWork.CompleteAsync() > 0)
                        return new ReturnDto<bool>(true, true, "Files saved successfully.");
                    else
                        return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex.Message}");
                    return new ReturnDto<bool>(false, false, ex.Message);
                }
            }

            return new ReturnDto<bool>(false, false, "No valid files uploaded.");
        }

    }
}
