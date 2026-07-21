using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Address
    {
        public Address()
        {
            Orderss = new HashSet<Orderss>();
        }

        public int AddressId { get; set; }
        public string AddressStreet { get; set; }
        public string IdUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SecondryPhone { get; set; }
        public string BuildingNo { get; set; }
        public int? ApartmentNo { get; set; }
        public string SpecialSign { get; set; }
        public bool? IsMain { get; set; }
        public int? Floor { get; set; }
        public int? City { get; set; }
        public byte? ShiftDelivary { get; set; }
        public int? GovernoratesId { get; set; }
        public int? NeighborhoodId { get; set; }
        public string BuildingNo1 { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public virtual Cities CityNavigation { get; set; }
        public virtual Governorates Governorates { get; set; }
        public virtual AspNetUsers IdUserNavigation { get; set; }
        public virtual Neighborhood Neighborhood { get; set; }
        public virtual ICollection<Orderss> Orderss { get; set; }
    }
}
