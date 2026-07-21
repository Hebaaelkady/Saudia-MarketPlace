using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kader.Data.DataAccessLayer.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(string id);
        ValueTask<TEntity> GetAsync(int id);
        TEntity Get(Guid id);
        ValueTask<TEntity> GetAsync(string id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, object>> predicate);
        Task<List<TEntity>> GetAllAsync(int pageIndex, int pageSize);
        Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, params Expression<Func<TEntity, object>>[] entityInclude);
        Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<T> Find<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> selected);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> selected);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> Include);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] entityInclude);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> Include);
        Task<TEntity> SingleOrDefaultAsync(
    Expression<Func<TEntity, bool>> predicate,
    params Expression<Func<TEntity, object>>[] includes);
        void Add(TEntity entity);
        Task<int> CountAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IList<TEntity> entities);
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity); // update entity
        Task UpdateAsync(TEntity entity); // update entity
        Task<int> CountAllItems();
        Task<IQueryable<TEntity>> GetAllQueryable(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> ToListAsync(IQueryable<TEntity> query);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int skip, int pageSize);
        IQueryable<TEntity> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate);

        Task<IQueryable<TEntity>> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate, bool preInclude, params Expression<Func<TEntity, object>>[] entityInclude);
        Task<IQueryable<TEntity>> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate, bool preInclude, int skip, int take, params Expression<Func<TEntity, object>>[] entityInclude);
    }
}
