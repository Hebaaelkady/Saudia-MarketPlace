using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Catogry
{
   public class Catogry_backendDto
    {
        public List<CatogryDto> ExistingItems { get; set; }
        public CatogryDto NewItem { get; set; }
    }
}
