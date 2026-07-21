using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class LogQuantityRepository : Repository<LogQuantity>, ILogQuantityRepository
    {
        

        public LogQuantityRepository(DbContext context) : base(context)
        {
        }

        
    }
}
