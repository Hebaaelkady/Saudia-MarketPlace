using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItems>();
        }

        public Guid? OrderPayment { get; set; }
        public string UserId { get; set; }
        public int? OrderNo { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorder { get; set; }
        public string CouponDiscount { get; set; }

        public virtual Address Address { get; set; }
        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
