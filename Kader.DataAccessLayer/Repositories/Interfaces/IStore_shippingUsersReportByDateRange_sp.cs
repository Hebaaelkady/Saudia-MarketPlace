using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;

namespace Kader.Data.DataAccessLayer.Repositories.Interfaces
{
    public interface IStore_shippingUsersReportByDateRange_sp : IRepository_SP<Store_shippingUsersReportByDateRange>
    {
        Task<List<Store_shippingUsersReportByDateRange>> GetList(params object[] orderedParameters);
        Task<Store_shippingUsersReportByDateRange> GetSingle(params string[] orderedParameters);
    }
}
