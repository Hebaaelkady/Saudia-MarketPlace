using Kader.DTOs.Payment;
using Kader.DTOs.ReturnOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Cart
{
    public class RefundRequestDto
    {
        public decimal Amount { get; set; }
        public string PaymentId { get; set; }
        public int p_id { get; set; }
        public List<ReturnsItemsDto> ReturnsItemsDto { get; set; }
    }
    public class ReturnsItemsDto
    {
        public int ProductId { get; set; }
        public int ApiProdId { get; set; }
       
        public string ProductName { get; set; } 
        public double? PriceItem { get; set; } 
        public int Quantity { get; set; }
    }
}
