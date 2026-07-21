using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Neighborhood
    {
        public Neighborhood()
        {
            Address = new HashSet<Address>();
        }

        public int NeighborhoodId { get; set; }
        public string Name { get; set; }
        public int? CityId { get; set; }
        public int? RegionId { get; set; }

        public virtual Cities City { get; set; }
        public virtual ICollection<Address> Address { get; set; }
    }
}
