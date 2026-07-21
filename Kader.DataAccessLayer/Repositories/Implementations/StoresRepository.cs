using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class StoresRepository : Repository<Stores>, IStoresRepository
    {
        

        public StoresRepository(DbContext context) : base(context)
        {
        }

        
    }
}
