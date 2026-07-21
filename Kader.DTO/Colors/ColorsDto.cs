using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Colors
{
   public class ColorsDto
    {
        public int ColorId { get; set; }
        public string ColorName  { get; set; }
        public string ColorDegree { get; set; }
    }
}
