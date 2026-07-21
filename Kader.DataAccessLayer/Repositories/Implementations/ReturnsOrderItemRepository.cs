using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ReturnsOrderItemRepository : Repository<ReturnsOrderItem>, IReturnsOrderItemRepository
    {
        

        public ReturnsOrderItemRepository(DbContext context) : base(context)
        {
        }

        
    }
}
