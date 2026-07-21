using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    
    public class ApplicationUser : IdentityUser, IEquatable<ApplicationUser>
    {
        public bool? IsDelete { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TypeUser { get; set; }
        public string CountryCode { get; set; }
        public bool Equals(ApplicationUser other)
        {
            // Implement your equality logic here, for example:
            return other != null && this.Id == other.Id;
        }

        // Override the base class Equals and GetHashCode methods as well:
        public override bool Equals(object obj)
        {
            return Equals(obj as ApplicationUser);
        }

        public override int GetHashCode()
        {
            // Use an appropriate hash code combination logic, for example:
            return this.Id.GetHashCode();
        }
    }
}
