using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Sms
{

    public interface ISmsService
    {
        Task<SendOtpResponse> SendOtpAsync(string mobileNumber);
        Task<bool> VerifyOtpAsync(string mobileNumber, string otpCode, int otpId);
        Task<SendOtpResponse> SendSmsAsync(string numbers,   string message);
    }
}
