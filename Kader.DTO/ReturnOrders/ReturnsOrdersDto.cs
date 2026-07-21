using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Orderss;
using Kader.DTOs.ReturnsOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnOrders
{


    public class ItemslistOrderDto
    {
        public string ProductName { get; set; }
        public double  PriceItem { get; set; }
        public double  ShippingPrice { get; set; }
        public int Quantity { get; set; }
        public int? typeGomlaOrQt3 { get; set; } //1 gomla  2 qt3
        public int OrderItemsId { get; set; }
        public bool HasExistingReturn { get; set; }
    }
    public class ReturnsOrderviewDto
    {
         
    public int ReturnsOrderId { get; set; } 
    public DateTime? RequestDate { get; set; }
    public int? StatusId { get; set; }
  
}
public class ReturnsOrdersDto
    {
        public AddressDto AddressDto { get; set; }
        public List<ItemslistOrderDto> Itemslist { get; set; }
        public int Idordersss { get; set; }
        public List<ReturnsOrderviewDto> ReturnsOrderview  { get; set; }
        public List<ReturnsOrderItemDto> ReturnsOrderItem  { get; set; }
    }

}
