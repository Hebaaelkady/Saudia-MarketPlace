using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Cart; 
using Kader.Services.Implementations.Cart;
using Kader.Services.Utilities.Mappers;
 
using System.Text.Json; 
using Microsoft.AspNetCore.Identity;
 
namespace Kader.Services.Implementations.CatCartType
{

    public class CartService : ICartService
    {
        private IUnitOfWork _unitOfWork; private readonly string _apiKey;
        private IMapper _mapper; private readonly HttpClient _httpClient;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CartService(HttpClient httpClient, IUnitOfWork unitOfWork, Microsoft.Extensions.Configuration.IConfiguration configuration, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork; _httpClient = httpClient;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage; 
            _apiKey = configuration["pk_live_wLVEej7pbtpKP76wpsA59eTfjjZCR8Vrpx3VFtkG"];
        }
        public async Task<RefundResponseDto> CreateRefundAsync(RefundRequestDto request)
        {
            var refundEndpoint = $"https://api.moyasar.com/v1/payments/{request.PaymentId}/refund";
           // string apiKey = "sk_test_EKLBA5Squ4jmeBB5adyBm7D4rGtDvok5C1RToxuh";  // Replace with your actual API key
            string apiKey = "sk_live_rgu2E2u42zGnDJBNadU3ecH3ukVeuf4p3Dnsw66L";  // Replace with your actual API key
            var byteArray = Encoding.ASCII.GetBytes($"{apiKey}:");
            var base64Auth = Convert.ToBase64String(byteArray);

            var jsonRequest = JsonSerializer.Serialize(new { amount = request.Amount });

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, refundEndpoint);
            requestMessage.Headers.Add("Authorization", $"Basic {base64Auth}");
            requestMessage.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponseBody = await response.Content.ReadAsStringAsync();

                    // Parse the error response and extract the message
                    using (JsonDocument doc = JsonDocument.Parse(errorResponseBody))
                    {
                        var message = doc.RootElement.GetProperty("message").GetString();

                        // Create and return the RefundResponseDto with the message
                        return new RefundResponseDto
                        {
                            Message1 = message,
                            IsSuccess = false
                        };
                    }
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ensure case insensitivity
                };
                var refundResponse = JsonSerializer.Deserialize<RefundResponseDto>(responseBody, options);


                refundResponse.IsSuccess = true;
                return refundResponse;
            }
            catch (HttpRequestException e)
            {
                return new RefundResponseDto
                {
                    IsSuccess = false,
                    Message1 = e.Message
                };
            }
            catch (Exception e)
            {
                return new RefundResponseDto
                {
                    IsSuccess = false,
                    Message1 = e.Message
                };
            }
        }
        // Helper method to retrieve error details if available.
        private async Task<string> GetErrorResponseAsync(HttpRequestException e)
{
    if (e.Data.Contains("ResponseBody"))
        return e.Data["ResponseBody"]?.ToString() ?? "No response body available.";
    return "No additional error details available.";
}
    }
}
