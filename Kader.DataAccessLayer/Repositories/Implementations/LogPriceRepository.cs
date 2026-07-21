using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class LogPriceRepository : Repository<LogPrice>, ILogPriceRepository
    {
        

        public LogPriceRepository(DbContext context) : base(context)
        {
        }

        
    }
}
