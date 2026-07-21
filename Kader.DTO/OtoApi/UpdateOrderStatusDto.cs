using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.OtoApi
{
    public class UpdateOrderStatusDto
    {
    
        public string[] orderIds { get; set; } 
        public string status { get; set; } 
        public string description { get; set; } 
        //public DateTime? date { get; set; }
    }
}
