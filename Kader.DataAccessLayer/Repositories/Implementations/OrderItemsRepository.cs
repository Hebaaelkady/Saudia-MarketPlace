using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class OrderItemsRepository : Repository<OrderItems>, IOrderItemsRepository
    {
        

        public OrderItemsRepository(DbContext context) : base(context)
        {
        }

        
    }
}
