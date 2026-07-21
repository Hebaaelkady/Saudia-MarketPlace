using Kader.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Cart
{
    // Models/RefundResponse.cs
    using System.Text.Json.Serialization;
    //public class RefundResponseDto
    //{
    //    public string Id { get; set; }
    //    public string Status { get; set; }
    //    [JsonPropertyName("refunded")]
    //    public int RefundedAmount { get; set; }
    //    [JsonPropertyName("refunded_at")]
    //    public string RefundedAt { get; set; }

    //    public bool IsSuccess { get; set; }
    //    public string Type { get; set; } 
    //    public object Errors { get; set; }
    //}
    public class RefundResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message1 { get; set; }
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("refunded")]
        public decimal Refunded { get; set; }

        [JsonPropertyName("refunded_at")]
        public DateTime? RefundedAt { get; set; }

        [JsonPropertyName("refunded_format")]
        public string RefundedFormat { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("source")]
        public RefundSourceDto Source { get; set; }
    }

    public class RefundSourceDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("gateway_id")]
        public string GatewayId { get; set; }

        [JsonPropertyName("reference_number")]
        public string ReferenceNumber { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("response_code")]
        public string ResponseCode { get; set; }

        [JsonPropertyName("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonPropertyName("issuer_name")]
        public string IssuerName { get; set; }

        [JsonPropertyName("issuer_card_type")]
        public string IssuerCardType { get; set; }

        [JsonPropertyName("issuer_card_category")]
        public string IssuerCardCategory { get; set; }
    }

}
