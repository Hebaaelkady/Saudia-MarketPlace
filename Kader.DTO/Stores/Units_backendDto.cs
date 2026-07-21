using Kader.DTOs.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Stores
{
   public class Stores_backendDto
    {
        public List<StoresDto> ExistingStores { get; set; }
        public StoresDto NewStores { get; set; }
    }
}
