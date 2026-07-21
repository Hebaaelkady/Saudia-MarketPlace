using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class ReturnsOrderItem
    {
        public int OrderItemsId { get; set; }
        public int? Reason { get; set; }
        public int? StatusId { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public int ReturnsOrderId { get; set; }
        public int ReturnsOrderItemId { get; set; }

        public virtual OrderItems OrderItems { get; set; }
        public virtual Reason ReasonNavigation { get; set; }
        public virtual ReturnsOrder ReturnsOrder { get; set; }
        public virtual Status Status { get; set; }
    }
}
