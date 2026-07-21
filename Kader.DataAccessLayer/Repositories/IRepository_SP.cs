using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kader.Data.DataAccessLayer.Repositories
{
    public interface IRepository_SP<TEntity> where TEntity : class
    {
        Task<TEntity> GetSingle(string storedProcedure, params string[] orderedParameters);
        Task<List<TEntity>> GetList(string storedProcedure, params string[] orderedParameters);
        List<TEntity> GetListTest(string storedProcedure, params string[] orderedParameters);
        IQueryable<TEntity> GetListTest1(string storedProcedure, params string[] orderedParameters);
        int CountList(string storedProcedure, params string[] orderedParameters);
    }
}
