using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class CitiesRepository : Repository<Cities>, ICitiesRepository
    {
        

        public CitiesRepository(DbContext context) : base(context)
        {
        }

        
    }
}
