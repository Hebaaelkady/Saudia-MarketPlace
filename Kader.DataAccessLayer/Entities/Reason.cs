using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Reason
    {
        public Reason()
        {
            ReturnsOrderItem = new HashSet<ReturnsOrderItem>();
        }

        public int ReasonId { get; set; }
        public string ReasonName { get; set; }

        public virtual ICollection<ReturnsOrderItem> ReturnsOrderItem { get; set; }
    }
}
