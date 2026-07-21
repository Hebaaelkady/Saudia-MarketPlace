using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.CatType;
using Kader.DTOs.Payment;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Implementations.Payment;
using Kader.Services.Utilities.Mappers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace Kader.Services.Implementations.Payment
{

    public class PaymentService : IPaymentService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public PaymentService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public  ReturnDto<PaymentDto> GetPayment(string id)
        {
            try
            { 

                var  getData =  _unitOfWork.Payment.SingleOrDefault(d => d.Id == id)  ;
                var map = _mapper.Map<PaymentDto>(getData);
                return new ReturnDto<PaymentDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<PaymentDto>(false, null, ex.Message);
            }
        }

        public async Task<ReturnDto<string>> SavePayment(HttpContext context, PaymentDto PaymentDto)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                Data.DataAccessLayer.Entities.Payment newCat1 = null;
                if (PaymentDto.Id != null)
                {
                      newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Payment>(PaymentDto);
                    newCat1.InsertedBy = stringUserId;
                     
                      _unitOfWork.Payment.Add(newCat1);
                }

                if (  _unitOfWork.Complete() > 0)
                    return new ReturnDto<string>(true, newCat1.Id, string.Empty);
                else
                    return new ReturnDto<string>(false, null, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<string>(false, null, ex.Message);
            }
        }
        public ReturnDto<bool> UpdatePayment(PaymentDto PaymentDto)
        {
            try
            {
                //   var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (PaymentDto.Id != null)
                {

                    var oldCat = _unitOfWork.Payment.SingleOrDefault(o => o.Id == PaymentDto.Id);
                    if (PaymentDto.Status != null)
                    {
                        oldCat.Status = PaymentDto.Status;
                    }
                    if (PaymentDto.Message != null)
                    {
                        oldCat.Message = PaymentDto.Message;
                    }
                    if (PaymentDto.StatusOrder != null)
                    {
                        oldCat.StatusOrder = PaymentDto.StatusOrder;
                    }
                    if (PaymentDto.StatusOrderItems != null)
                    {
                        oldCat.StatusOrderItems = PaymentDto.StatusOrderItems;
                    }
                    _unitOfWork.Payment.Update(oldCat);
                }

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
        }
        public async Task<ReturnDto<bool>> UpdatePayment(HttpContext context, RefundResponseDto PaymentDto, int id)
        {
            try
            {
                Data.DataAccessLayer.Entities.Payment newCat1 = null;
                if (PaymentDto.Id != null)
                {
                    var oldCat = await _unitOfWork.Payment.GetAsync(id);
                    //var oldCat =await _unitOfWork.Payment.SingleOrDefaultAsync(o => o.PId == id);
                    if (PaymentDto.IsSuccess == false)
                    {
                        oldCat.Description = PaymentDto.Message1;
                        oldCat.RefundSuccess = false;
                        PaymentDto.RefundedAt = DateTime.UtcNow.AddHours(3);
                       await _unitOfWork.Payment.UpdateAsync(oldCat);
                    }
                    else
                    {
                        var newCat = _mapper.Map(PaymentDto, oldCat);
                        //newCat1 = _mapper.Map<Data.DataAccessLayer.Entities.Payment>(PaymentDto);
                        newCat.PId = id;
                       await _unitOfWork.Payment.UpdateAsync(newCat);
                    }
                }

                if (await _unitOfWork.CompleteAsync() > 0)
                {
                    return new ReturnDto<bool>(true, true, string.Empty);
                }
                else
                {
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> DeletePayment(HttpContext context, string id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Payment.SingleOrDefaultAsync(l => l.Id == id);
                 
                await _unitOfWork.Payment.UpdateAsync(newCat);

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
