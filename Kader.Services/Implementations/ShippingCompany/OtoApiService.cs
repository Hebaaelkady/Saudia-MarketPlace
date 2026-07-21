
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Kader.DTOs.OtoApi; 
using Kader.Infrastructure.Jwt.Interfaces;
using Kader.Data.DataAccessLayer;
using AutoMapper;
using Kader.Services.Utilities.Mappers;  
using Kader.Data.DataAccessLayer.Entities; 
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using System.Net.Http;
using Newtonsoft.Json; 
using Kader.Services.Implementations.ShippingCompany;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
namespace Kader.Services.Implementations.User
{
    public class OtoApiService : IOtoApiService
    {
        private readonly string _refreshToken;
        private string _accessToken;
        private DateTime _accessTokenExpiry;
        private readonly ILogger  _logger;
        private readonly HttpClient _httpClient;
        private IUnitOfWork _unitOfWork;
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
         private IKeys _keys;
        private readonly IConfiguration _configuration;
        private IMapper _mapper;
        private const int MaxRetryAttempts = 3;
        private const int DelayBetweenRetriesMs = 1000;
        public OtoApiService(ILogger  logger, HttpClient httpClient, IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration, IJwtFactory jwtFactory)
        {
            _httpClient = httpClient;
            _jwtFactory = jwtFactory;
            _unitOfWork = unitOfWork;  
            _userManager = userManager;
            _roleManager = roleManager; _keys = new Keys();
            _configuration = configuration;
            _refreshToken = configuration["OtoApi:RefreshToken"];
            if (string.IsNullOrEmpty(_refreshToken))
            {
                throw new Exception("RefreshToken is not configured in appsettings1.json.");
            }
            _mapper = ObjectMapper.Mapper;
        }


        public async Task<string> GetAccessTokenAsync()
        {
            // Check if the access token is still valid
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow.AddHours(3) < _accessTokenExpiry)
            {
                return _accessToken;
            }

            // Fetch a new access token
            using var client = new HttpClient();
            var requestBody = new { refresh_token = _refreshToken };
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.tryoto.com/rest/v2/refreshToken", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponseDto>(responseContent);

            if (tokenResponse.Success)
            {
                _accessToken = tokenResponse.AccessToken;
                _accessTokenExpiry = DateTime.UtcNow.AddHours(3).AddSeconds(tokenResponse.ExpiresIn);
                return _accessToken;
            }

            throw new Exception("Failed to fetch access token.");
        }

        public async Task<List<DeliveryOptionDto>> GetDeliveryOptionsAsync(string accessToken)
        {
            const string url = "https://api.tryoto.com/rest/v2/getDeliveryOptions";

            // Set up HTTP client and authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Send GET request
            var response = await _httpClient.GetAsync(url);

            // Log the response for debugging
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Content: {responseContent}");

            // Check if the response was successful
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<DeliveryOptionDto>>(responseContent);
            }
            else
            {
                throw new Exception($"API call failed with status code {response.StatusCode}. Content: {responseContent}");
            }

            throw new Exception("Unexpected response format or failed to fetch delivery options.");
        }

        private async Task<string> PostAsync1(string url, object payload, string accessToken)
        {
            try
            {
                // Ensure the HttpClient has proper authorization
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Serialize the payload to JSON
                var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                // Log request details
                Console.WriteLine($"POST URL: {url}");
                Console.WriteLine($"Payload: {JsonConvert.SerializeObject(payload)}");

                // Send POST request
                var response = await _httpClient.PostAsync(url, content);

                // Read and log response body
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Body: {responseBody}");

                // Check for non-successful responses and handle gracefully
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {responseBody}");

                    // Handle specific status codes
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.BadRequest:
                            throw new HttpRequestException("Bad Request: Check your payload or API endpoint.");
                        case System.Net.HttpStatusCode.Unauthorized:
                            throw new HttpRequestException("Unauthorized: Check your access token.");
                        default:
                            throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {responseBody}");
                    }
                }

                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                // Log error and rethrow to handle upstream
                Console.WriteLine($"HTTP Request failed: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Catch other unexpected exceptions
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }



        //private async Task<string> PostAsync(string url, object payload, string accessToken)
        //{
        //    // Ensure the access token is valid
        //    if (string.IsNullOrEmpty(accessToken))
        //    {
        //        throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        //    }

        //    // Log the attempt and response details within RetryAsync
        //    return await RetryAsync(
        //        async () => await ExecutePostRequest(url, payload, accessToken),
        //        maxRetries: 3,
        //        delayInMilliseconds: 2000 // Retry every 2 seconds
        //    );
        //}

        //// Encapsulate the actual POST request logic
        //private async Task<string> ExecutePostRequest(string url, object payload, string accessToken)
        //{
        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        // Serialize the payload to JSON
        //        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        //        // Log request details
        //        _logger.LogInformation("POST URL: {Url}", url);
        //        _logger.LogInformation("Payload: {Payload}", JsonConvert.SerializeObject(payload));

        //        // Send POST request
        //        var response = await _httpClient.PostAsync(url, content);

        //        // Read and log the response body
        //        var responseBody = await response.Content.ReadAsStringAsync();
        //        _logger.LogInformation("Response Status: {StatusCode}", response.StatusCode);
        //        _logger.LogInformation("Response Body: {ResponseBody}", responseBody);

        //        // Handle unsuccessful responses
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            _logger.LogWarning("Unsuccessful Response: {StatusCode} - {ResponseBody}", response.StatusCode, responseBody);

        //            switch (response.StatusCode)
        //            {
        //                case System.Net.HttpStatusCode.BadRequest:
        //                    throw new HttpRequestException("Bad Request: Check your payload or API endpoint.");
        //                case System.Net.HttpStatusCode.Unauthorized:
        //                    throw new HttpRequestException("Unauthorized: Check your access token.");
        //                default:
        //                    throw new HttpRequestException($"HTTP Error: {response.StatusCode} - {responseBody}");
        //            }
        //        }

        //        // Deserialize and validate the JSON response
        //        dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
        //        if (jsonResponse.success != true)
        //        {
        //            throw new Exception($"API call failed: {jsonResponse.message}");
        //        }

        //        return responseBody; // Return the response body on success
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during POST to {Url}", url);
        //        throw; // Propagate the error for retry logic
        //    }
        //}

        //// RetryAsync method for retrying the operation
        //private async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries, int delayInMilliseconds)
        //{
        //    int retryCount = 0;

        //    while (retryCount < maxRetries)
        //    {
        //        try
        //        {
        //            return await action(); // Try executing the action
        //        }
        //        catch (Exception ex)
        //        {
        //            retryCount++;
        //            _logger.LogWarning(ex, "Retry attempt {RetryCount} failed.", retryCount);

        //            if (retryCount >= maxRetries)
        //            {
        //                _logger.LogError(ex, "All retry attempts failed.");
        //                throw; // Rethrow if maximum retries are reached
        //            }

        //            await Task.Delay(delayInMilliseconds); // Wait before retrying
        //        }
        //    }

        //    throw new Exception("Operation failed after maximum retries."); // Fallback in case of logic issues
        //}



        public async Task<string> CheckOTODeliveryFeeAsync(CheckOTODeliveryFeeRequestDto requestDto, string accessToken)
        {
            const string requestUrl = "https://api.tryoto.com/rest/v2/checkOTODeliveryFee";

             
                var requestBody = JsonConvert.SerializeObject(requestDto);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                return await PostAsync(requestUrl, content, accessToken);
            }
        //public async Task<string> CreatePickupLocationAsync(PickupLocationDto pickupLocationDto, string accessToken)
        //{
        //    const string url = "https://api.tryoto.com/rest/v2/createPickupLocation";

        //    try
        //    {
        //        var response = await PostAsync(url, pickupLocationDto, accessToken);
        //        Console.WriteLine("Pickup Location Created Successfully.");
        //        return response; // Return the response for further processing
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error creating pickup location: {ex.Message}");
        //        throw;
        //    }
        //}

        //// Create Order
        //public async Task<string> CreateOrderAsync(CreateOrderDto orderDto, string accessToken)
        //{
        //    const string url = "https://api.tryoto.com/rest/v2/createOrder";

        //    try
        //    {
        //        var response = await PostAsync(url, orderDto, accessToken);
        //        Console.WriteLine("Order Created Successfully.");
        //        return response; // Return the response for further processing
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error creating order: {ex.Message}");
        //        throw;
        //    }
        //}

        //// Create Shipment
        //public async Task<string> CreateShipmentAsync(CreateShipmentDto shipmentDto, string accessToken)
        //{
        //    const string url = "https://api.tryoto.com/rest/v2/createShipment";

        //    try
        //    {
        //        var response = await PostAsync(url, shipmentDto, accessToken);
        //        Console.WriteLine("Shipment Created Successfully.");
        //        return response; // Return the response for tracking or further processing
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error creating shipment: {ex.Message}");
        //        throw;
        //    }
        //}








        public async Task<string> CreatePickupLocationAsync(PickupLocationDto pickupLocationDto, string accessToken)
        {
            var requestUrl = "https://api.tryoto.com/rest/v2/createPickupLocation";
            var requestBody = JsonConvert.SerializeObject(pickupLocationDto);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await PostAsync(requestUrl, content, accessToken);
        }

          public async Task<string> CreateOrderAsync(CreateOrderDto createOrderDto, string accessToken)
        {
            var requestUrl = "https://api.tryoto.com/rest/v2/createOrder";
            var requestBody = JsonConvert.SerializeObject(createOrderDto);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await PostAsync(requestUrl, content, accessToken);
        }

         public async Task<string> CreateShipmentAsync(CreateShipmentDto createShipmentDto, string accessToken)
        {
            var requestUrl = "https://api.tryoto.com/rest/v2/createShipment";
            var requestBody = JsonConvert.SerializeObject(createShipmentDto);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await PostAsync(requestUrl, content, accessToken);
        }

        private async Task<string> PostAsync(string url, HttpContent content, string accessToken)
        {
            int attempt = 0;
            string responseContent = null;

            // Set Authorization header with token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            while (attempt < MaxRetryAttempts)
            {
                attempt++;

                try
                {
                    var response = await _httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                    else
                    {
                        //"{\"orderId\":\"1174\",\"otoErrorCode\":\"OTO1063\",\"success\":false,\"errorCode\":14,\"otoErrorMessage\":\"Order Id is already exist\",\"errorMsg\":\"This order exists.\"}"
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Attempt {attempt}: Received {response.StatusCode} - {response.ReasonPhrase}=");
                        responseContent = $"Attempt {attempt} failed with status code {response.StatusCode}: {response.ReasonPhrase}: {errorContent}";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during HTTP Post (Attempt {attempt}): {ex.Message}");
                    responseContent = $"Error during HTTP Post (Attempt {attempt}): {ex.Message}";
                }

                // Delay before the next retry attempt
                await Task.Delay(DelayBetweenRetriesMs);
            }

            // Return a failure message if all retries fail
            return responseContent ?? "All retry attempts failed. No response received.";
        }

        public async Task<string> GetPickupLocationList( string accessToken,string? minDate = null, string? maxDate = null, string? status = "active")
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (!string.IsNullOrEmpty(minDate) && !DateTime.TryParse(minDate, out _))
            {
                throw new ArgumentException("Invalid minDate format. Use 'yyyy-MM-dd'.");
            }

            if (!string.IsNullOrEmpty(maxDate) && !DateTime.TryParse(maxDate, out _))
            {
                throw new ArgumentException("Invalid maxDate format. Use 'yyyy-MM-dd'.");
            }

            if (!string.IsNullOrEmpty(status) && !(status.Equals("active", StringComparison.OrdinalIgnoreCase) || status.Equals("inactive", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid status. Use 'active' or 'inactive'.");
            }

            var url = "https://api.tryoto.com/rest/v2/getPickupLocationList";
            status = "active";
            var query = "?";
            if (!string.IsNullOrEmpty(minDate)) query += $"minDate={minDate}&";
            if (!string.IsNullOrEmpty(maxDate)) query += $"maxDate={maxDate}&";
            if (!string.IsNullOrEmpty(status)) query += $"status={status}&";

            query = query.TrimEnd('&');

            try
            {
                var response = await _httpClient.GetAsync(url + query);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API responded with status code: {response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching data: {ex.Message}", ex);
            }
        }

        public async Task<string> GetShipmentDetailsAsync(string accessToken, string orderId)
        {
            string responseContent = null;
            // Validate inputs
            if (string.IsNullOrWhiteSpace(accessToken)) 
            responseContent = $"Access token is required.";
            if (string.IsNullOrWhiteSpace(orderId)) 
            responseContent = $"Order ID is required.";
            // Define the endpoint
            var url = "https://api.tryoto.com/rest/v2/orderStatus";

            // Set up the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            // Set up the headers
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            // Create the JSON body
            var requestBody = new { orderId };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                var response = await _httpClient.PostAsync(url, jsonContent);

                // Check if the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    responseContent = $"Error: {response.StatusCode}, Content: {errorContent}";
                   // throw new HttpRequestException($"Error: {response.StatusCode}, Content: {errorContent}");
                }

                // Return the response content
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            { 
            responseContent = $"Content: {ex.Message}";
           // throw new Exception($"An error occurred while fetching the shipment details: {ex.Message}", ex);
            }
            return responseContent ?? "All retry attempts failed. No response received.";
        }
        public async Task<string> Update_Order_Status(UpdateOrderStatusDto UpdateOrderStatus, string accessToken)
        {
            var requestUrl = "https://api.tryoto.com/rest/v2/updateOrderStatus";
            var requestBody = JsonConvert.SerializeObject(UpdateOrderStatus);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await PostAsync(requestUrl, content, accessToken);
        }

        public async Task<string> UpdateOrderStatusAsync(string accessToken)
        {
            string requestUrl = "https://api.tryoto.com/rest/v2/updateOrderStatus";
            //string accessToken = "your-access-token"; // Replace with a valid token

            var requestBody = new
            {
                orderIds = new[] { 1138 },
                status = "delivered",
                date = DateTime.UtcNow.AddHours(3).ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                description = "Shipment delivered successfully",
                userId = "12345"

            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    return errorResponse;
                }
            }
        }

        public async Task<string> createReturnShipment(createReturnShipmentDto createReturnShipment, string accessToken)
        {
            var requestUrl = "https://api.tryoto.com/rest/v2/createReturnShipment";
            var requestBody = JsonConvert.SerializeObject(createReturnShipment);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            return await PostAsync(requestUrl, content, accessToken);
        }
    }
}




