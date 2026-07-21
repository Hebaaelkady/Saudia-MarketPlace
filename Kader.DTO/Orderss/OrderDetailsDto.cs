using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.OrderStatus;
using Kader.DTOs.ShippingPrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Orderss
{
    public class OrderDetailsDto
    {
        public AddressDto AddressDto { get; set; }
        public List<Itemslist> Itemslist { get; set; }
        public List<OrderStatusDto> OrderStatus { get; set; }
        public int? StatusId { get; set; }
        public string OrdersNo { get; set; }
        //OrdersPayment
        public string id { get; set; }
        public int P_Id { get; set; }
        public double? amount { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
         public int Idordersss { get; set; }
        public string PaymentID { get; set; }
        public string ShippingotoId { get; set; }
        public string ShippingotoMessge { get; set; }
        
             public string InsideShippingUserFirstName { get; set; }
        public string InsideShippingUserLastName { get; set; }
        public string InsideShippingUserPhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string CountryCode { get; set; }
        public string UserId1 { get; set; }
        public string UserId { get; set; }
        public string CouponDiscount { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public bool MsgCofeToUser { get; set; }
        public bool MsgToDelivery { get; set; }
        public ApiShippingorderStatus ApiShippingorderStatus { get; set; }
    }
    public class ApiShippingorderStatus
    {
        public string status { get; set; }
        public bool success { get; set; }
        public string deliveryCompany { get; set; }
        public string note { get; set; }
        public DateTime date { get; set; }
        public DateTime deliverySlotDate { get; set; }
        public string shipmentId { get; set; }
        public string printAWBURL { get; set; }
        public int otoId { get; set; }
    }
}
