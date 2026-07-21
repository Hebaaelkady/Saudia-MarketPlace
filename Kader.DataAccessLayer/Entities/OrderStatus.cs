using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class OrderStatus
    {
        public int OrderStatusId { get; set; }
        public int? OrderId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public int? StatusId { get; set; }
        public int? StoreId { get; set; }
        public string Comment { get; set; }

        public virtual Orderss Order { get; set; }
        public virtual Status Status { get; set; }
        public virtual Stores Store { get; set; }
    }
}
