using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Address
{
   public class AddressDto
    {


        public int AddressId { get; set; }
        [Required(ErrorMessage = " مطلوب.")]
        public string AddressStreet { get; set; }
        public string IdUser { get; set; }
        public string SecondryPhone { get; set; }
        [Required(ErrorMessage = " مطلوب.")]
        public string BuildingNo { get; set; }
        public int? ApartmentNo { get; set; }
        public string SpecialSign { get; set; }
        public bool? IsMain { get; set; }
        public int? Floor { get; set; }
        public int? City { get; set; }
        public byte? ShiftDelivary { get; set; }
        public int? GovernoratesID { get; set; }
        [Required(ErrorMessage = " مطلوب.")]
        public int? NeighborhoodId { get; set; }
        public string Governoratesname { get; set; }
        public string BuildingNo1 { get; set; }
        public string CityName { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
