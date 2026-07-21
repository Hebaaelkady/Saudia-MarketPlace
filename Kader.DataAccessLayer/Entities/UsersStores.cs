using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class UsersStores
    {
        public int UsersStoresId { get; set; }
        public string UserId { get; set; }
        public int? StoreId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? Shipinng { get; set; }
        public bool? Storing { get; set; }

        public virtual Stores Store { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
