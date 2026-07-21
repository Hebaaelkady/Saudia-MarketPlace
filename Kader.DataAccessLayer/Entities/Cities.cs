using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Cities
    {
        public Cities()
        {
            Address = new HashSet<Address>();
            Neighborhood = new HashSet<Neighborhood>();
        }

        public int CityId { get; set; }
        public string CityName { get; set; }
        public int? GovernorateId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual Governorates Governorate { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Neighborhood> Neighborhood { get; set; }
    }
}
