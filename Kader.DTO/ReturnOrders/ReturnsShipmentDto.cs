using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Address;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Orderss;
using Kader.DTOs.OtoApi;
using Kader.DTOs.ReturnsOrderItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnOrders
{


    public class ReturnsShipmentDto
    {
        public AcceptReturnsOrdersDto AcceptReturnsOrdersDto { get; set; }
        public bool AcceptReject { get; set; }
        public UpdateOrderStatusDto UpdateOrderStatus { get; set; }
        public createReturnShipmentDto createReturnShipment { get; set; }
       
    }
  

}
