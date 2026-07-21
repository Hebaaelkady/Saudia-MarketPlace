using Kader.DTOs.RoleDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{
 
    public class ForgotPasswordDto
    {
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
    }
}
