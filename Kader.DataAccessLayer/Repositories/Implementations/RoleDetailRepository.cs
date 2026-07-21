using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class RoleDetailRepository : Repository<RoleDetail>, IRoleDetailRepository
    {
        

        public RoleDetailRepository(DbContext context) : base(context)
        {
        }

        
    }
}
