using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Catogry
{
   public class CatogryDto
    {
        public int CatogryId { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string CatogryName { get; set; }
        public int? CatId { get; set; }
        public string CatName { get; set; }
        public int? CatLevel { get; set; }
        public int? TypeId { get; set; }

        public string RoleId { get; set; }
    }
}
