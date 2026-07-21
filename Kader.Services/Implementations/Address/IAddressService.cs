using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Address
{

    public interface IAddressService
    {
        Task<ReturnDto<List<AddressDto>>> GetAddress(HttpContext context);
        Task<ReturnDto<bool>> SaveAddress(HttpContext context, AddressDto AddressDto);
        Task<ReturnDto<bool>> DeleteAddress(HttpContext context, int id);
        Task<ReturnDto<AddressDto>> GetSingleAddress(int id);
        Task<ReturnDto<AddressDto>> GetActiveAddress(HttpContext context);
    }
}
