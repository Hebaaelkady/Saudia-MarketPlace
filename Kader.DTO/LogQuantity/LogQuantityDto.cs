using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.LogQuantity
{
   public class LogQuantityDto
    {
        public int LogQuantityId { get; set; }
        public int? ProductId { get; set; }
        public string InsertedBy { get; set; } 
        public double? RemainingQuantity { get; set; }
        public double? NewQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
