using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class CatTypeRepository : Repository<CatType>, ICatTypeRepository
    {
        

        public CatTypeRepository(DbContext context) : base(context)
        {
        }

        
    }
}
