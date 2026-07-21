using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.ShippingPrice;
using Kader.DTOs.Product;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.ShippingPrice
{

    public class ShippingPriceService : IShippingPriceService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper; 
        private readonly RoleManager<ApplicationRole> _roleManager;
        public ShippingPriceService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        public async Task<ReturnDto<bool>> SaveShippingPrice(HttpContext context, IList<ShippingPriceDto> ShippingPrices, bool IsODD_Even, int? ProductID)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (ShippingPrices == null || ProductID == null || ProductID == 0)
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred!");

                // Fetch existing shipping prices for this ProductID
                var existingPrices = await _unitOfWork.ShippingPrice
                    .FindAsync(sp => sp.ProductId == ProductID && sp.IsDeleted==false);

                // If IsODD_Even is true, mark all old records as deleted except the first one
                if (IsODD_Even && existingPrices.Any())
                {
                    var firstRow = existingPrices.First(); // Keep one row for updating
                    foreach (var oldPrice in existingPrices.Where(sp => sp.ShippingPriceId != firstRow.ShippingPriceId))
                    {
                        oldPrice.IsDeleted = true;
                        oldPrice.UpdateDate = DateTime.UtcNow.AddHours(3);
                        oldPrice.UpdateBy = stringUserId;
                        await _unitOfWork.ShippingPrice.UpdateAsync(oldPrice);
                    }
                }

                foreach (var shippingPriceDto in ShippingPrices)
                {
                    if (shippingPriceDto.VarPrice == null || shippingPriceDto.VarQuantity == null)
                        continue; // Skip invalid entries

                    if (shippingPriceDto.ShippingPriceId == 0) // New record
                    {
                        var newPrice = _mapper.Map<Data.DataAccessLayer.Entities.ShippingPrice>(shippingPriceDto);
                        newPrice.InsertedBy = stringUserId;
                        newPrice.ProductId = ProductID;
                        newPrice.IsOddEven = IsODD_Even;
                        await _unitOfWork.ShippingPrice.AddAsync(newPrice);
                    }
                    else // Update existing record
                    {
                        var existingPrice = await _unitOfWork.ShippingPrice.GetAsync(shippingPriceDto.ShippingPriceId);
                        if (existingPrice != null)
                        {
                            _mapper.Map(shippingPriceDto, existingPrice);
                            existingPrice.UpdateDate = DateTime.UtcNow.AddHours(3);
                            existingPrice.IsOddEven = IsODD_Even;
                            existingPrice.UpdateBy = stringUserId;
                            existingPrice.ProductId = ProductID;
                            await _unitOfWork.ShippingPrice.UpdateAsync(existingPrice);
                        }
                    }
                }

                if (await _unitOfWork.CompleteAsync() > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<List<ShippingPriceDto>>> GetShippingPrice(HttpContext context, int ProductId)
        {
            var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            try
            {
                List<Data.DataAccessLayer.Entities.ShippingPrice> getData = await _unitOfWork.ShippingPrice.FindAsync(o => o.IsDeleted == false &&o.ProductId==ProductId) ?? new List<Data.DataAccessLayer.Entities.ShippingPrice>();
                var map = _mapper.Map<List<ShippingPriceDto>>(getData);
                return new ReturnDto<List<ShippingPriceDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<ShippingPriceDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> DeleteAllShippingPrice(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                List<Data.DataAccessLayer.Entities.ShippingPrice> getData = await _unitOfWork.ShippingPrice.FindAsync(o => o.IsDeleted == false && o.ProductId == id) ?? new List<Data.DataAccessLayer.Entities.ShippingPrice>();
                foreach (var Products in getData)
                {
                    Products.DeletedBy = stringUserId;
                    Products.IsDeleted = true;
                    Products.DeletedDate = DateTime.UtcNow.AddHours(3);
                    await _unitOfWork.ShippingPrice.UpdateAsync(Products);
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


        public async Task<ReturnDto<bool>> DeleteShippingPrice(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.ShippingPrice.SingleOrDefaultAsync(l => l.ShippingPriceId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.ShippingPrice.UpdateAsync(newCat);

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

    }
}
