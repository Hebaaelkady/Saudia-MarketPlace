using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ReturnsOrderRepository : Repository<ReturnsOrder>, IReturnsOrderRepository
    {
        

        public ReturnsOrderRepository(DbContext context) : base(context)
        {
        }

        
    }
}
