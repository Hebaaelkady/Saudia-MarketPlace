using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Payment
{
   public class PaymentDto
    {
        public string Id { get; set; }
        public string Status { get; set; } 
        public double? Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? StatusOrder { get; set; }
        public int? StatusOrderItems { get; set; }

    }
}
