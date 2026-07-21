using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Orderss;
using Kader.DTOs.OrderStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnOrders
{
    public class ReturnsOrderItemsDetailDto
    {
        public int ProductId { get; set; }
        public int ApiProdId { get; set; }
        public int? Reason { get; set; }
        public int? StatusId { get; set; }
        public string ProductName { get; set; }
        public string unit { get; set; }
        public string sku { get; set; }
        public double? PriceItem { get; set; }
        public double? ShippingPrice { get; set; }
        public int Quantity { get; set; }
    }
    public class ReturnOrdersDetailsDto
    {
        
        public int ReturnsOrderId { get; set; }
        public AddressDto AddressDto { get; set; }
        public List<ReturnsOrderItemsDetailDto> ReturnsOrderItemsDetail { get; set; }
        public List<OrderStatusDto> OrderStatusw { get; set; }
        public int OrderNo { get; set; }
        public int Idordersss { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTransportShippPrice { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? AddressId { get; set; }
        public int Idorders { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public int StatusOrderId { get; set; }
        public string ShippingotoId { get; set; }
        public string ShippingotoMessge { get; set; }
        public Guid OrderPayment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public ApiShippingorderStatus ApiShippingorderStatus { get; set; }
    }
}
