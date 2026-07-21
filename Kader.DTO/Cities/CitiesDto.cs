using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Cities
{
   public class CitiesDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? GovernorateId { get; set; }

    }
}
