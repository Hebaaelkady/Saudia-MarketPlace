using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.RoleDetail
{
   public class RoleDetailDto
    {
        public int RoleDetaailId { get; set; }
        //public bool? CanRead { get; set; }
        //public bool? CanAdd { get; set; }
        //public bool? CanUpdate { get; set; }
        //public bool? CanDelete { get; set; }
        public string URL { get; set; }
        public string RoleId { get; set; }
    }
}
