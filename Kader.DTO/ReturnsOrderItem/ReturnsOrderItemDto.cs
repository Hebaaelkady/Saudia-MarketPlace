using Kader.Data.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ReturnsOrderItem
{
   public class ReturnsOrderItemDto
    { 
        public int? OrderItemsId { get; set; } 
        public int? Reason { get; set; }
        public int? StatusId { get; set; } 
        public int? ReturnsOrderId { get; set; } 
        public bool Selected { get; set; }
    }
}
