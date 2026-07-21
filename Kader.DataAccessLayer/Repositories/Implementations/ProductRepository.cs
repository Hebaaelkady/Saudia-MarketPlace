using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        

        public ProductRepository(DbContext context) : base(context)
        {
        }

        
    }
}
