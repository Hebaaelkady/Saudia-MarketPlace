using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class GovernoratesRepository : Repository<Governorates>, IGovernoratesRepository
    {
        

        public GovernoratesRepository(DbContext context) : base(context)
        {
        }

        
    }
}
