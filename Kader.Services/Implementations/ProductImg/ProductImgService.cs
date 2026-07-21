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
using Kader.DTOs.ProductImg;
using Kader.DTOs.Product;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.Extensions.FileProviders;
namespace Kader.Services.Implementations.ProductImg
{

    public class ProductImgService : IProductImgService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ProductImgService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        
        public async Task<ReturnDto<List<ProductImgDto>>> GetProductImg(HttpContext context, int ProductId)
        {
           
            try
            {
                List<Data.DataAccessLayer.Entities.ProductImg> getData = await _unitOfWork.ProductImg.FindAsync(o => o.IsDeleted == false && o.ProductId == ProductId) ?? new List<Data.DataAccessLayer.Entities.ProductImg>();

                var map = _mapper.Map<List<ProductImgDto>>(getData);
                return new ReturnDto<List<ProductImgDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ProductImgDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> AddFiles(HttpContext context, List<IFormFile> FormFiles, int ProductId)
        {
            List<string> x = new List<string>();
            if (ProductId != 0)
            {
                foreach (var file in FormFiles)
                {
                    int maxFileSizeInBytes = 3 * 1024 * 1024; // 1MB

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
                            var newCat = _mapper.Map<Data.DataAccessLayer.Entities.ProductImg>(new ProductImgDto { ProductImgName = newFileName, ProductId = ProductId });

                            await _unitOfWork.ProductImg.AddAsync(newCat);
                        }
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
    
        public string AddFile(IFormFile file, string name)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var newFileName = String.Concat(name, fileExtension);
            var filepath = "";

            filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files")).Root + $@"\{newFileName}";

            using (FileStream fs = System.IO.File.Create(filepath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return newFileName;
        }
        public async Task<ReturnDto<bool>> DeleteAllFile(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.ProductImg> getData = await _unitOfWork.ProductImg.FindAsync(o => o.IsDeleted == false && o.ProductId == id) ?? new List<Data.DataAccessLayer.Entities.ProductImg>();
                foreach (var Products in getData)
                {
                    await _unitOfWork.ProductImg.RemoveAsync(Products);
                    var fileExtension = Path.GetExtension(Products.ProductImgName);
                    var filepath = "";

                    filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{fileExtension}";

                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
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


        public async Task<ReturnDto<bool>> DeleteFile(HttpContext context, int ProductImgId)
        {
            var ProductImgcourse = await _unitOfWork.ProductImg.GetAsync(ProductImgId);
            await _unitOfWork.ProductImg.RemoveAsync(ProductImgcourse);
            if (_unitOfWork.Complete() > 0)
            {
                var fileExtension = Path.GetExtension(ProductImgcourse.ProductImgName);
                var filepath = "";

                filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{fileExtension}";

                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                return new ReturnDto<bool>(true, true);
            }
               
            else
                return new ReturnDto<bool>(false, false, "error saved");

            
            
        }

    }
}
