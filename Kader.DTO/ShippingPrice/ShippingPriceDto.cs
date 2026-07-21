using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ShippingPrice
{
   public class ShippingPriceDto
    {
        public bool IsODD_Even { get; set; }

        public int ShippingPriceId { get; set; }
        public int? ProductId { get; set; }
        public bool IsDeleted { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "مسموح ارقام فقط.")]
        public double? VarQuantity { get; set; }
        [RegularExpression(@"^[0-9]*(\.[0-9]+)?$", ErrorMessage = "مسموح بالأرقام والأرقام العشرية فقط.")]
        public double? VarPrice { get; set; }
    }
}
