using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        

        public AddressRepository(DbContext context) : base(context)
        {
        }

        
    }
}
