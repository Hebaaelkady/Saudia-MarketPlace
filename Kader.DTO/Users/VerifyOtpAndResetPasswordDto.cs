using Kader.DTOs.RoleDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{

    public class VerifyOtpAndResetPasswordDto
    {
        public string Mobile { get; set; }
        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string Otp { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; }
    }

}
