using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.OrderStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Orderss
{
   public class OrderBackendDto
    {

        public AddressDto AddressDto { get; set; }
        // public ProductItemDto ProductItemDto { get; set; }
            public List<Itemslist> Itemslist { get; set; }
        public List<OrderStatusDto> OrderStatus { get; set; }
        public int OrderNo { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorders { get; set; }

        public string status { get; set; }

        public Guid OrderPayment { get; set; }
        public string UserId { get; set; }
        public string CouponDiscount { get; set; }
        public string OrderSource { get; set; }
        public int StoreId { get; set; }
        
    }
}
