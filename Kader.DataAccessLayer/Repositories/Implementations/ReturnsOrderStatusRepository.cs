using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ReturnsOrderStatusRepository : Repository<ReturnsOrderStatus>, IReturnsOrderStatusRepository
    {
        

        public ReturnsOrderStatusRepository(DbContext context) : base(context)
        {
        }

        
    }
}
