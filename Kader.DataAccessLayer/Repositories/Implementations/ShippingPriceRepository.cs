using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ShippingPriceRepository : Repository<ShippingPrice>, IShippingPriceRepository
    {
        

        public ShippingPriceRepository(DbContext context) : base(context)
        {
        }

        
    }
}
