using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class CatogryRepository : Repository<Catogry>, ICatogryRepository
    {
        

        public CatogryRepository(DbContext context) : base(context)
        {
        }

        
    }
}
