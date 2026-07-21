using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            Catogry = new HashSet<Catogry>();
            RoleDetail = new HashSet<RoleDetail>();
        }

        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public int? CatIdaPi { get; set; }
        public int? CatTypeId { get; set; }

        public virtual CatType CatType { get; set; }
        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual ICollection<Catogry> Catogry { get; set; }
        public virtual ICollection<RoleDetail> RoleDetail { get; set; }
    }
}
