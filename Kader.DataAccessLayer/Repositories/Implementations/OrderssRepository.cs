using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class OrderssRepository : Repository<Orderss>, IOrderssRepository
    {
        

        public OrderssRepository(DbContext context) : base(context)
        {
        }

        
    }
}
