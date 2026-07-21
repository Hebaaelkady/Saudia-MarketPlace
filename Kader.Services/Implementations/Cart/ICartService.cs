using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Cart;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Cart
{

    public interface ICartService
    {
        Task<RefundResponseDto> CreateRefundAsync(RefundRequestDto request);
    }
 
}
