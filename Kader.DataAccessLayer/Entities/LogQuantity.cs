using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class LogQuantity
    {
        public int LogQuantityId { get; set; }
        public int? ProductId { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public double? RemainingQuantity { get; set; }
        public double? NewQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
