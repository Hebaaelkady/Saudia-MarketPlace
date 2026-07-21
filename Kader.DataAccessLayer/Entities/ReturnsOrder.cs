using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class ReturnsOrder
    {
        public ReturnsOrder()
        {
            ReturnsOrderItem = new HashSet<ReturnsOrderItem>();
            ReturnsOrderStatus = new HashSet<ReturnsOrderStatus>();
        }

        public int ReturnsOrderId { get; set; }
        public int OrderId { get; set; }
        public string ReturnMethod { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? StatusId { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? insideShipping { get; set; }
        public bool? ShippingReturnBool { get; set; }
        public string ShippingReturnotoMessge { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Comment { get; set; }
        public int? StoreId { get; set; }

        public virtual AspNetUsers InsertedByNavigation { get; set; }
        public virtual Orderss Order { get; set; }
        public virtual Status Status { get; set; }
        public virtual Stores Store { get; set; }
        public virtual ICollection<ReturnsOrderItem> ReturnsOrderItem { get; set; }
        public virtual ICollection<ReturnsOrderStatus> ReturnsOrderStatus { get; set; }
    }
}
