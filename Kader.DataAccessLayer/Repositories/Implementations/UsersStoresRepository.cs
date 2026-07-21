using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class UsersStoresRepository : Repository<UsersStores>, IUsersStoresRepository
    {
        

        public UsersStoresRepository(DbContext context) : base(context)
        {
        }

        
    }
}
