using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Kader.DTOs.Payment; 
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Payment
{

    public interface IPaymentService
    {
       ReturnDto<PaymentDto> GetPayment(string id);
        Task<ReturnDto<string>> SavePayment(HttpContext context, PaymentDto PaymentDto);
        Task<ReturnDto<bool>> DeletePayment(HttpContext context, string id);
        ReturnDto<bool> UpdatePayment(  PaymentDto PaymentDto);
        Task<ReturnDto<bool>> UpdatePayment(HttpContext context, RefundResponseDto PaymentDto, int id);

    }
}
