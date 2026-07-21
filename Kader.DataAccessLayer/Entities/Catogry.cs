using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Catogry
    {
        public Catogry()
        {
            InverseCat = new HashSet<Catogry>();
            Product = new HashSet<Product>();
        }

        public int CatogryId { get; set; }
        public string CatogryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public int? CatLevel { get; set; }
        public int? CatId { get; set; }
        public int? TypeId { get; set; }
        public string RoleId { get; set; }

        public virtual Catogry Cat { get; set; }
        public virtual AspNetRoles Role { get; set; }
        public virtual ICollection<Catogry> InverseCat { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
