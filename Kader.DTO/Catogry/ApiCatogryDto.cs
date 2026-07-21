using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Catogry
{
   public class ApiCatogryDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nameEn { get; set; } // Add this property
        public string nameTy { get; set; }
 
        public int? CatTypeID { get; set; }
        public int? CatIDaPI { get; set; }
    }
}
