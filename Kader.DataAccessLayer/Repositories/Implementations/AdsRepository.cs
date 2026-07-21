using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class AdsRepository : Repository<Ads>, IAdsRepository
    {
        

        public AdsRepository(DbContext context) : base(context)
        {
        }

        
    }
}
