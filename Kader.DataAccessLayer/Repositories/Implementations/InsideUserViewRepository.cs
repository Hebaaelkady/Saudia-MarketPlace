using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class InsideUserViewRepository : Repository<InsideUserView>, IInsideUserViewRepository
    {
        

        public InsideUserViewRepository(DbContext context) : base(context)
        {
        }

        
    }
}
