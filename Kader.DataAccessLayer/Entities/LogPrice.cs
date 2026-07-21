using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class LogPrice
    {
        public int LogPriceId { get; set; }
        public int? ProductId { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DiscountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public double? BeforeDiscount { get; set; }
        public double? AfterDiscount { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Product Product { get; set; }
    }
}
