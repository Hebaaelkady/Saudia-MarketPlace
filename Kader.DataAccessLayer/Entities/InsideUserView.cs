using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class InsideUserView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Count1 { get; set; }
        public int? CountStatus14 { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
