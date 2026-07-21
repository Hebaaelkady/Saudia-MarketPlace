using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Stores
{
   public class StoresDto
    {

        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
    }
}
