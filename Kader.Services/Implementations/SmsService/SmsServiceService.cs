using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer;
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Services.Implementations.ShippingCompany;
using Kader.Services.Implementations.Sms;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SmsService : ISmsService
{
    private readonly HttpClient _httpClient; private IUnitOfWork _unitOfWork;
    private readonly string _apiUrl = "https://www.msegat.com/gw";
    private readonly string _userName = "jinamarket"; private readonly IJwtFactory _jwtFactory;
    private readonly string _apiKey = "3933588302C0BF2FFA99558B9DBE2285";
    private readonly string _userSender = "JINA";
    public SmsService(ILogger logger, HttpClient httpClient, IUnitOfWork unitOfWork, IJwtFactory jwtFactory)
    {
        _httpClient = httpClient;
        _jwtFactory = jwtFactory;
        _unitOfWork = unitOfWork;
        
    }


    public async Task<SendOtpResponse> SendSmsAsync(string numbers,   string message)
    {
       
        var payload = new
        {
            userName = _userName,
            apiKey = _apiKey,
            numbers = numbers,
            userSender = _userSender,
            msg = message,
            msgEncoding = "UTF8"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiUrl}/sendsms.php", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        //"{\"code\":\"1061\",\"message\":\"\"}"
        var responseContent = await response.Content.ReadAsStringAsync();
        responseContent = responseContent.Replace("\"code\":\"", "\"code\":").Replace("\",", ",");
        return JsonSerializer.Deserialize<SendOtpResponse>(responseContent);
    }


    // ✅ Send OTP
    public async Task<SendOtpResponse> SendOtpAsync(string mobileNumber)
    {
        var requestData = new
        {
            lang = "En",
            userName = _userName,
            number = mobileNumber,
            apiKey = _apiKey,
            userSender = _userSender
        };
  
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiUrl}/sendOTPCode.php", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<SendOtpResponse>(responseContent);
    }


    // ✅ Verify OTP
    public async Task<bool> VerifyOtpAsync(string mobileNumber, string otpCode, int otpId)
    {
        var requestData = new
        {
            lang = "En",
            userName = _userName,
            apiKey = _apiKey,
            code = otpCode,
            id = otpId,
            userSender = _userSender
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_apiUrl}/verifyOTPCode.php", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
      

        var verificationResponse = JsonSerializer.Deserialize<VerifyOtpResponse>(responseContent);

        return verificationResponse.code == 1;
    }

}

// ✅ Response Models
public class SendOtpResponse
{
    //"{\"code\":1,\"message\":\"Success\",\"id\":1235301}"
    public int code { get; set; }
    public string message { get; set; }
    public int id { get; set; }
} 
public class VerifyOtpResponse
{
    public int code { get; set; }
    public string message { get; set; }
}
