using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class OrderItems
    {
        public int OrderItemsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double PriceItem { get; set; }
        public double? ShippingPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertBy { get; set; }
        public int? ColorId { get; set; }
        public int? UnitId { get; set; }
        public string ProductName { get; set; }
        public int? StatusId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public int? CatIdApi { get; set; }
        public int? TypeGomlaOrQt3 { get; set; }

        public virtual Colors Color { get; set; }
        public virtual Orderss Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual Status Status { get; set; }
        public virtual Units Unit { get; set; }
        public virtual ReturnsOrderItem ReturnsOrderItem { get; set; }
    }
}
