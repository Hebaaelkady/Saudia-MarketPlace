using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Colors
{
   public class Colors_backendDto
    {
        public List<ColorsDto> ExistingItems { get; set; }
        public ColorsDto NewItem { get; set; }
    }
}
