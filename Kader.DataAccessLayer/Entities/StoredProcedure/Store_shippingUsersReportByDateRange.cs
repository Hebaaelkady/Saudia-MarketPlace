using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.Data.DataAccessLayer.Entities.StoredProcedure
{
   public class Store_shippingUsersReportByDateRange
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public int? TotalOrders { get; set; }
        public int? Count_Status_12 { get; set; }
        public int? Count_Status_3 { get; set; }
        public int? Expr13 { get; set; }
        public int? Expr14 { get; set; }
        public int? Expr4 { get; set; }
      

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
