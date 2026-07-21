using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.ProductImg
{
   public class ProductImgDto
    {
        public string ProductImgName { get; set; }
        public int? ProductId { get; set; }
        public int ProductImgId { get; set; }
    }
}
