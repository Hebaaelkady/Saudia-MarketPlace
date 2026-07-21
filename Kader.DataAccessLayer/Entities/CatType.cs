using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class CatType
    {
        public CatType()
        {
            AspNetRoles = new HashSet<AspNetRoles>();
            Product = new HashSet<Product>();
        }

        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
