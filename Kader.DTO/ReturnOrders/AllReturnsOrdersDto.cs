using Kader.DTOs.Address;
using Kader.DTOs.OrderItems;
using Kader.DTOs.ReturnsOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnOrders
{
  
    public class AllReturnsOrdersDto
    {

        public AddressDto AddressDto { get; set; }
        // public ProductItemDto ProductItemDto { get; set; }
         public List<ReturnsOrderItemslistDto> ReturnsOrderItemslistDto { get; set; }

        public int ReturnsOrderId { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorders { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }

        public Guid OrderPayment { get; set; }
        public string UserId { get; set; }
        public string CouponDiscount { get; set; }

    }
}
