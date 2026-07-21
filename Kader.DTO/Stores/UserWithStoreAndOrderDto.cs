using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Stores
{

    public class UserWithStoreAndOrderDto
    {
        
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<StoreDto> Stores { get; set; } = new List<StoreDto>();
        
    }

    public class StoreDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int NewAssignedCount { get; set; }  // StatusId = 12
        public int ShippedOrderCount { get; set; } // StatusId = 4
        public int UndercheckedCount { get; set; } // 3
    }



}
