using Kader.DTOs.Address;
using Kader.DTOs.RoleDetail;
using Kader.DTOs.UsersStores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kader.DTOs.Users
{
 
    public class UserPhoneDto
    { 
        public int TypeUser { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Id { get; set; } 
        public string Username { get; set; }
         
    }
}
