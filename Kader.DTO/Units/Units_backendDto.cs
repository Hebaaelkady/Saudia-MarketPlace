using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Units
{
   public class Units_backendDto
    {
        public List<UnitsDto> ExistingItems { get; set; }
        public UnitsDto NewItem { get; set; }
    }
}
