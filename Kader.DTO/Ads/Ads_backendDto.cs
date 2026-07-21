using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Ads
{
   public class Ads_backendDto
    {
        public List<AdsDto> ExistingItems { get; set; }
        public AdsDto NewItem { get; set; }
    }
}
