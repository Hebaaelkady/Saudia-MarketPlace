using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.OrderStatus
{
   public class OrderStatusDto
    {
        public int OrderStatusId { get; set; }
        public int? OrderId { get; set; } 
        public DateTime? CreatedDate { get; set; } 
        public int? StatusId { get; set; }
        public int? Status  { get; set; }
        public string StoreName { get; set; }
        public string Comment { get; set; }
        public int StoreID { get; set; }
        public string InsertedBy { get; set; }
    }
}
