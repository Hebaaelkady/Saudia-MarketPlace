using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ProductImgRepository : Repository<ProductImg>, IProductImgRepository
    {
        

        public ProductImgRepository(DbContext context) : base(context)
        {
        }

        
    }
}
