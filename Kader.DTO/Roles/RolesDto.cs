using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Roles
{
   public class RolesDto
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Name { get; set; }
        public int? CatIdaPi { get; set; }
        public int? CatTypeId { get; set; }
    }
}
