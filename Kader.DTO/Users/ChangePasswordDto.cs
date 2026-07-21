using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Users
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        public string Id {  get; set; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "تاكيد كلمة السر الجديدة غير مطابقة لكلمة السر")]

        [Required(ErrorMessage = "تاكيد كلمة المرور .")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
