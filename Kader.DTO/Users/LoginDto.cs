using Kader.DTOs.RoleDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{
 
    public class LoginDto
    {
        public int KaderuserId { get; set; }
        [Required(ErrorMessage = "رقم التليفون مطلوب.")]
        [RegularExpression(@"^(\+?(\d{1,3})[- .]?)?((\d{1,4})[- .]?){2,6}$", ErrorMessage = "يجب أن يكون رقم التليفون صالحًا.")]

        public string userName { get; set; }
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [DataType(DataType.Password)]
        public string Userpass { get; set; }
        public string Password { get; set; }
        public RoleDetailDto RoleDetailDto { get; set; }
        public List<AllUserViewDto> AllUserViewDto { get; set; }
    }
}
