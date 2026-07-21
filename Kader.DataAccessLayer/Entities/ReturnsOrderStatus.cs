using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class ReturnsOrderStatus
    {
        public int ReturnsOrderStatusId { get; set; }
        public int? ReturnsOrderId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public int? StatusId { get; set; }
        public int? StoreId { get; set; }

        public virtual ReturnsOrder ReturnsOrder { get; set; }
        public virtual Status Status { get; set; }
        public virtual Stores Store { get; set; }
    }
}
