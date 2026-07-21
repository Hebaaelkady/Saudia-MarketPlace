using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Orderss;
using Kader.DTOs.ReturnsOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnOrders
{


     
 
public class saveReturnsOrdersDto
    {
        public AddressDto AddressDto { get; set; } 

        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
 
        public int? AddressId { get; set; }
        public int Idordersss { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string CouponDiscount { get; set; }
    
        public int Selected { get; set; } 
        public List<ReturnsOrderItemDto> ReturnsOrderItem  { get; set; }
    }

}
