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


    public class RejectReturnsOrdersDto
    {
        public string comment { get; set; } 
        public int ReturnsOrderId { get; set; }
        public int status { get; set; } = 0; //1 gomla  2 qt3
       
    }
     
}
