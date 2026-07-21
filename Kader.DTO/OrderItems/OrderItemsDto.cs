using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.OrderItems
{
   public class OrderItemsDto
    {
        public int OrderItemsId { get; set; }
        public int? OrderId { get; set; }
        public string ProductName { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? PriceItem { get; set; }
        public double? ShippingPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertBy { get; set; }
        public int? ColorId { get; set; }
        public int? UnitId { get; set; }
        public int? TypeGomlaOrQt3 { get; set; }
    }
}
