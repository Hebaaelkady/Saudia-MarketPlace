using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.OtoApi
{
    public class createReturnShipmentDto
    { 
        public string orderId { get; set; } 
        public string pickupLocationCode { get; set; } 
        public string deliveryOptionId { get; set; }
        public List<ItemsDto> items { get; set; }
    }
    public class ItemsDto
    {
        public string quantity { get; set; }
        public int sku { get; set; } 
    }
}
