using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class StoreShippingUsersReportRepository : Repository<StoreShippingUsersReport>, IStoreShippingUsersReportRepository
    {
        

        public StoreShippingUsersReportRepository(DbContext context) : base(context)
        {
        }

        
    }
}
