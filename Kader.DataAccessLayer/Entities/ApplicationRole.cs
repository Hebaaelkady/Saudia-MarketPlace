using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kader.Data.DataAccessLayer.Entities
{
    public class ApplicationRole : IdentityRole 
    {
        public int? CatTypeID { get; set; }
        public int? CatIDaPI { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public ApplicationRole() : base()
        {
            // Your additional properties and methods here
        }

        public ApplicationRole(string roleName, int? catTypeID = null, int? catIDaPI = null)
        : base(roleName) // Pass roleName to the base IdentityRole constructor
        {
            CatTypeID = catTypeID;
            CatIDaPI = catIDaPI;
            IsDelete = false; // Default value
        }
    }

}
