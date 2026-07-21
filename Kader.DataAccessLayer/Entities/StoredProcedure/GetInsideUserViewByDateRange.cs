using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.Data.DataAccessLayer.Entities.StoredProcedure
{
   public class GetInsideUserViewByDateRange
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? COUNT1 { get; set; }

        public int? COUNT_Status_14 { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public string PhoneNumber { get; set; }

    }
}
