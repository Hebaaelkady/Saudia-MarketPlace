using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Status
    {
        public Status()
        {
            OrderItems = new HashSet<OrderItems>();
            OrderStatus = new HashSet<OrderStatus>();
            Orderss = new HashSet<Orderss>();
            PaymentStatusOrderItemsNavigation = new HashSet<Payment>();
            PaymentStatusOrderNavigation = new HashSet<Payment>();
            ReturnsOrder = new HashSet<ReturnsOrder>();
            ReturnsOrderItem = new HashSet<ReturnsOrderItem>();
            ReturnsOrderStatus = new HashSet<ReturnsOrderStatus>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<Orderss> Orderss { get; set; }
        public virtual ICollection<Payment> PaymentStatusOrderItemsNavigation { get; set; }
        public virtual ICollection<Payment> PaymentStatusOrderNavigation { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrder { get; set; }
        public virtual ICollection<ReturnsOrderItem> ReturnsOrderItem { get; set; }
        public virtual ICollection<ReturnsOrderStatus> ReturnsOrderStatus { get; set; }
    }
}
