using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class ShippingPrice
    {
        public int ShippingPriceId { get; set; }
        public int? ProductId { get; set; }
        public double VarQuantity { get; set; }
        public double VarPrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool? IsOddEven { get; set; }

        public virtual Product Product { get; set; }
    }
}
