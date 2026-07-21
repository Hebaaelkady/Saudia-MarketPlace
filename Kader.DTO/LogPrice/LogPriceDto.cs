using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.LogPrice
{
   public class LogPriceDto
    {
        public int LogPriceId { get; set; }
        public int? ProductId { get; set; }

        public double? AfterDiscount { get; set; }
        public double? BeforeDiscount { get; set; }
        public DateTime? discountBeginDate { get; set; }
        public DateTime? discountEndDat { get; set; }
    }
}
