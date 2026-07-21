using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class NeighborhoodRepository : Repository<Neighborhood>, INeighborhoodRepository
    {
        

        public NeighborhoodRepository(DbContext context) : base(context)
        {
        }

        
    }
}
