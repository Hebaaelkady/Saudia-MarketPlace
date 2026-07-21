using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.ShippingPrice;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.ShippingPrice
{

    public interface IShippingPriceService
    {
        Task<ReturnDto<List<ShippingPriceDto>>> GetShippingPrice(HttpContext context, int ProductId);
        Task<ReturnDto<bool>> SaveShippingPrice(HttpContext context, IList<ShippingPriceDto> ShippingPrice, bool IsODD_Even,int? ProductID);
        Task<ReturnDto<bool>> DeleteShippingPrice(HttpContext context, int id);
        Task<ReturnDto<bool>> DeleteAllShippingPrice(HttpContext context, int id);
    }
}
