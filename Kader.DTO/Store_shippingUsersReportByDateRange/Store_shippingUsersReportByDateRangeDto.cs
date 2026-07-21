using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Store_shippingUsersReportByDateRange
{
   public class Store_shippingUsersReportByDateRangeDto
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string RoleName { get; set; }
        public int? TotalOrders { get; set; }
        public int? CountStatus12 { get; set; }
        public int? CountStatus3 { get; set; }
        public int? Expr13 { get; set; }
        public int? Expr14 { get; set; }
        public int? Expr4 { get; set; }
        public int? Expr15 { get; set; }
        public int? Expr16 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
