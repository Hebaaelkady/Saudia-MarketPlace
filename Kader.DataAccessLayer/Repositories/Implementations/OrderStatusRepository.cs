using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        

        public OrderStatusRepository(DbContext context) : base(context)
        {
        }

        
    }
}
