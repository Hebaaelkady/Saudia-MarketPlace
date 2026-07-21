using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            Address = new HashSet<Address>();
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            OrderssInsideShippingUserNavigation = new HashSet<Orderss>();
            OrderssUser = new HashSet<Orderss>();
            Payment = new HashSet<Payment>();
            ReturnsOrder = new HashSet<ReturnsOrder>();
            UsersStores = new HashSet<UsersStores>();
        }

        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TypeUser { get; set; }
        public int? VerifyCode { get; set; }
        public bool? ApprovedVerify { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CountryCode { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<Orderss> OrderssInsideShippingUserNavigation { get; set; }
        public virtual ICollection<Orderss> OrderssUser { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<ReturnsOrder> ReturnsOrder { get; set; }
        public virtual ICollection<UsersStores> UsersStores { get; set; }
    }
}
