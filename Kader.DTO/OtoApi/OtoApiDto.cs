using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kader.DTOs.OtoApi
{
    public class TokenResponseDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }


    public class DeliveryOptionDto
    {
        public string DeliveryOptionId { get; set; }
        public string CompanyName { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; } = false;
    }
    public class CheckOTODeliveryFeeRequestDto
    {
        public string originCity { get; set; }
        public string destinationCity { get; set; }
        public double weight { get; set; } // e.g., in kilograms
        public string serviceType { get; set; }   // e.g., "document" or "parcel"
        public int Idordersss { get; set; }
    }
    public class CheckOTODeliveryFeeResponseDto
    {
        public string TraceId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<DeliveryCompanyDto> DeliveryCompany { get; set; }
       
    }
    public class DeliveryCompanyDto
    {
         
        public string deliveryCompanyName { get; set; }
        public int deliveryOptionId { get; set; }
        public string pickupCutOffTime { get; set; }
        public bool Success { get; set; }
        public double price { get; set; }
        public double maxFreeWeight { get; set; }
        public double returnFee { get; set; }
        public double avgDeliveryTime { get; set; }
        
    }
  

    public class ShipmentRequestDto
    {
        public PickupLocationDto PickupLocationDto { get; set; }
        public CreateOrderDto CreateOrderDto { get; set; }
        public CreateShipmentDto CreateShipmentDto { get; set; }
    }

    public class PickupLocationDto
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string contactName { get; set; }
        public string mobile { get; set; }
        public string contactEmail { get; set; }
        public string code { get; set; }
        public string country { get; set; }
    }

    public class CreateOrderDto
    {
        
               public int Idordersss { get; set; }
        public string orderId { get; set; }
        public string payment_method { get; set; }
        public double amount { get; set; }
        public double shippingAmount { get; set; }
        public double amount_due { get; set; }
        public double subtotal { get; set; }
        
             public string senderName { get; set; }
        public string currency { get; set; }
        public string pickupLocationCode { get; set; }
        public string deliveryOptionId { get; set; }
        public string shippingNotes { get; set; }
        public string orderDate { get; set; }
        public string deliverySlotFrom { get; set; }
        public string deliverySlotTo { get; set; }
        public bool createShipment { get; set; }
        public CustomerDto customer { get; set; }
        public List<ItemDto> items { get; set; }
    }

    public class CustomerDto
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string district { get; set; }
        public double? lat { get; set; }
        public double? lon { get; set; }

    }

    public class ItemDto
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double rowTotal { get; set; }
        public string sku { get; set; }
    }

    public class CreateShipmentDto
    {
        public string orderId { get; set; }
        public string deliveryOptionId { get; set; } 
    }
     

    // Model for deserializing API response
    public class PickupLocationResponse
    {
        public bool Success { get; set; }
        public List<Location> Warehouses { get; set; }
        public List<Location> Branches { get; set; }
    }

    public class Location
    {
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
    }

}
