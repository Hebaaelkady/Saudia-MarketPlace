using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ApplicationRoleRepository : Repository<ApplicationRole>, IApplicationRoleRepository
    {
        

        public ApplicationRoleRepository(DbContext context) : base(context)
        {
        }

        
    }
}
