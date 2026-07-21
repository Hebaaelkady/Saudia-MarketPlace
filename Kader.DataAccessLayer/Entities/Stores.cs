using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Stores
    {
        public Stores()
        {
            OrderStatus = new HashSet<OrderStatus>();
            Orderss = new HashSet<Orderss>();
            ReturnsOrder = new HashSet<ReturnsOrder>();
            ReturnsOrderStatus = new HashSet<ReturnsOrderStatus>();
            UsersStores = new HashSet<UsersStores>();
        }

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string Address { get; set; }
        public virtual ICollection<OrderStatus> OrderStatus { get; set; }
        public virtual ICollection<Orderss> Orderss { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrder { get; set; }
        public virtual ICollection<ReturnsOrderStatus> ReturnsOrderStatus { get; set; }
        public virtual ICollection<UsersStores> UsersStores { get; set; }
    }
}
