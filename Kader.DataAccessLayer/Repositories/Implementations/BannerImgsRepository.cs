using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{
    public class BannerImgsRepository : Repository<BannerImgs>, IBannerImgsRepository
    {
        

        public BannerImgsRepository(DbContext context) : base(context)
        {
        }

        
    }
}
