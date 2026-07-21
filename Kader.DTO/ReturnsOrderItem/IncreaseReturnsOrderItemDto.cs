using Kader.Data.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnsOrderItem
{
   public class IncreaseReturnsOrderItemDto
    { 
       
        public int? Quantity { get; set; }
        public int? ProductId { get; set; } 
        public int? ReturnsOrderId { get; set; } 
       
    }
}
