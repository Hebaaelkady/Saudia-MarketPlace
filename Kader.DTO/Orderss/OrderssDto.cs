using Kader.DTOs.Address;
using Kader.DTOs.OrderItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Orderss
{
   public class OrderssDto
    {

        public AddressDto AddressDto { get; set; }
        // public ProductItemDto ProductItemDto { get; set; }
        // public List<OrderItemsDto> Itemslist { get; set; }

        public int OrderNo { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorders { get; set; }
        public string Status { get; set; }


        public Guid OrderPayment { get; set; }
        public string UserId { get; set; }
        public string CouponDiscount { get; set; }

    }
}
