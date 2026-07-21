using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Governorates
    {
        public Governorates()
        {
            Address = new HashSet<Address>();
            Cities = new HashSet<Cities>();
        }

        public int GovernorateId { get; set; }
        public string GovernorateName { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
    }
}
