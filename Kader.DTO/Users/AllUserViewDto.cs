using Kader.DTOs.Address;
using Kader.DTOs.RoleDetail;
using Kader.DTOs.UsersStores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{
 
    public class ApiRegisterDto
    {
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        public string Password { get; set; }
        public int TypeUser { get; set; }
        [Required(ErrorMessage = "الاسم الأول مطلوب.")]
        public string FirstName { get; set; }
        [EmailAddress(ErrorMessage = "يجب أن يكون البريد الإلكتروني صالحًا.")]
        public string Email { get; set; } = "default@example.com";
        [Required(ErrorMessage = "الاسم الأخير مطلوب.")]
        public string LastName { get; set; }
       
        [Required(ErrorMessage = "رقم التليفون مطلوب.")]
        [RegularExpression(@"^0?[1-9][0-9]{7,14}$", ErrorMessage = "يجب إدخال رقم هاتف صحيح.")]


        public string Username { get; set; }
         
        public string CountryCode { get; set; }
          
    }
}
