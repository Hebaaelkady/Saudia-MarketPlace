using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Microsoft.EntityFrameworkCore;
using System;

namespace Kader.Data.DataAccessLayer
{
    public partial class DBContext
    {
      
        public virtual DbSet<GetInsideUserViewByDateRange> GetInsideUserViewByDateRange { get; set; }
        public virtual DbSet<Store_shippingUsersReportByDateRange> Store_shippingUsersReportByDateRange { get; set; }
    }
}
