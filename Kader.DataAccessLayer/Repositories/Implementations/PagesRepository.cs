using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class PagesRepository : Repository<Pages>, IPagesRepository
    {
        

        public PagesRepository(DbContext context) : base(context)
        {
        }

        
    }
}
