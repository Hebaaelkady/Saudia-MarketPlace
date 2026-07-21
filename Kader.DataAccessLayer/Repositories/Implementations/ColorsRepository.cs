using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class ColorsRepository : Repository<Colors>, IColorsRepository
    {
        

        public ColorsRepository(DbContext context) : base(context)
        {
        }

        
    }
}
