using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class UnitsRepository : Repository<Units>, IUnitsRepository
    {
        

        public UnitsRepository(DbContext context) : base(context)
        {
        }

        
    }
}
