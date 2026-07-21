using Kader.DTOs.Address;
using Kader.DTOs.OrderItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Orderss
{
   public class MsgDto
    {
      
        public int Idorders { get; set; }
        public bool MsgCofeToUser { get; set; }
        public bool MsgToDelivery { get; set; }

    }
}
