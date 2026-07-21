using Kader.DTOs.Address;
using Kader.DTOs.OrderItems;
using Kader.DTOs.OrderStatus;
using Kader.DTOs.ShippingPrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Cart
{
    public class SaveCheckoutDto
    {
        public int Idorders { get; set; }
        public int StatusId { get; set; }
        public AddressDto AddressDto { get; set; } 
        public List<Itemslist> Itemslist { get; set; }
        public string PaymentId { get; set; }
        //OrdersPayment
        public string id { get; set; }
        public double? amount { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
     


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public string UserId { get; set; }
        public string CouponDiscount { get; set; }

    }
     
}
