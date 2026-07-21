using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ReturnsReasonRepository : Repository<ReturnsReason>, IReturnsReasonRepository
    {
        

        public ReturnsReasonRepository(DbContext context) : base(context)
        {
        }

        
    }
}
