using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class TransportMethod
    {
        public TransportMethod()
        {
            Product = new HashSet<Product>();
        }

        public int TransportMethodId { get; set; }
        public string TransportMethodName { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }
}
