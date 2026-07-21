using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;

namespace Kader.Data.DataAccessLayer.Repositories.Interfaces
{
    public interface IGetInsideUserViewByDateRange_sp : IRepository_SP<GetInsideUserViewByDateRange>
    {
        Task<List<GetInsideUserViewByDateRange>> GetList(params object[] orderedParameters);
        Task<GetInsideUserViewByDateRange> GetSingle(params string[] orderedParameters);
    }
}
