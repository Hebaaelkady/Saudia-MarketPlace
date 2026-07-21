 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Kader.DTOs.Users;
using Kader.DTOs;
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.DTOs.OtoApi;

namespace Kader.Services.Implementations.ShippingCompany
{
    public interface IOtoApiService
    {
        Task<string> GetAccessTokenAsync();
        Task<List<DeliveryOptionDto>> GetDeliveryOptionsAsync(string accessToken);
        Task<string> CheckOTODeliveryFeeAsync(CheckOTODeliveryFeeRequestDto requestDto, string accessToken);
        Task<string> CreatePickupLocationAsync(PickupLocationDto pickupLocationDto, string accessToken);
        Task<string> CreateOrderAsync(CreateOrderDto orderDto, string accessToken);
        Task<string> CreateShipmentAsync(CreateShipmentDto shipmentDto, string accessToken);
        Task<string> GetPickupLocationList(string accessToken, string? minDate = null, string? maxDate = null, string? status = null);
        Task<string> GetShipmentDetailsAsync(string accessToken, string orderId);
        Task<string> Update_Order_Status(UpdateOrderStatusDto UpdateOrderStatus, string accessToken);
        Task<string> createReturnShipment(createReturnShipmentDto createReturnShipment, string accessToken);
        Task<string> UpdateOrderStatusAsync(string accessToken);

    }
}




