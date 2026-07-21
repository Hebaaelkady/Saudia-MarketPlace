using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Neighborhood
{
   public class NeighborhoodDto
    {
        public int NeighborhoodId { get; set; }
        public string Name { get; set; }
        public int? CityId { get; set; }
        public string RegionId { get; set; }

    }
}
