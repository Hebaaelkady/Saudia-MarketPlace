using Kader.DTOs.Address;
using Kader.DTOs.RoleDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{
 
    public class Register_backendDto
    {
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        public string Password { get; set; }
        public int TypeUser { get; set; }
        
        public string Id { get; set; }
        [Required(ErrorMessage = "اسم المستخدم مطلوبة.")]
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStore { get; set; }
        public List<int> SelectedStoreIds { get; set; } = new List<int>();
    }
}
