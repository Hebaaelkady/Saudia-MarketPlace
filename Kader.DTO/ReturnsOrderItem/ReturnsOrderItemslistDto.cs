using Kader.Data.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnsOrderItem
{
   public class ReturnsOrderItemslistDto
    {
        public string ProductName { get; set; }
        public double? PriceItem { get; set; }
        public double? ShippingPrice { get; set; }
        public int Quantity { get; set; }
    }
}
