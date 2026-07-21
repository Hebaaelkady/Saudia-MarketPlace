using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Orderss
    {
        public Orderss()
        {
            OrderItems = new HashSet<OrderItems>();
            OrderStatus = new HashSet<OrderStatus>();
            ReturnsOrder = new HashSet<ReturnsOrder>();
        }

        public string PaymentId { get; set; }
        public string UserId { get; set; }
        public string OrdersNo { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorders { get; set; }
        public string CouponDiscount { get; set; }
        public int? StatusId { get; set; }
        public string ShippingotoId { get; set; }
        public string ShippingotoMessge { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? ShippingReturnBool { get; set; }
        public bool? MsgToDelivery { get; set; }
        public bool? MsgCofeToUser { get; set; }
        public int? StoreId { get; set; }
        public string InsideShippingUser { get; set; }

        public virtual Address Address { get; set; }
        public virtual AspNetUsers InsideShippingUserNavigation { get; set; }
        public virtual Status Status { get; set; }
        public virtual Stores Store { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrder { get; set; }
    }
}
