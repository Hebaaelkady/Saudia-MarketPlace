using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Kader.Data.DataAccessLayer.Repositories
{
    public class Repository<TEntity>:IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }


        public void CopyPropertiesTo<T, TU>(T source, TU dest)
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties().Where(x => x.CanRead).ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    try
                    {
                        var p = destProps.First(x => x.Name == sourceProp.Name);
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                    catch
                    { }
                }
            }
        }

        public TEntity Get(string id)
        {
            return Context.Find<TEntity>(id);
        }

        public ValueTask<TEntity> GetAsync(int id)
        {
            return Context.FindAsync<TEntity>(id);
        }
        public TEntity Get(Guid id)
        {
            return Context.Find<TEntity>(id);
        }

        public ValueTask<TEntity> GetAsync(string id)
        {
            return Context.FindAsync<TEntity>(id);
        }
        public Task<List<TEntity>> GetAllAsync()
        {
            return Context.Set<TEntity>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, object>> predicate)
        {
            var q = Context.Set<TEntity>().Select(predicate);
            var ret = await q.ToListAsync();
            List<T> result = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ret));
            return result;
        }

        public Task<List<TEntity>> GetAllAsync(int pageIndex, int pageSize)
        {
            return Context.Set<TEntity>().Skip(pageIndex).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await Context.Set<TEntity>().CountAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return 0;
            }
        }


        public async Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            var q = Context.Set<TEntity>().Where(predicate).Skip(pageIndex).Take(pageSize);
            var ret = await q.ToListAsync();
            List<T> result = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ret));
            return result;
        }  public async Task<List<T>> GetAllAsync<T>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, params Expression<Func<TEntity, object>>[] entityInclude)
        {
            var q = Context.Set<TEntity>().Where(predicate).Skip(pageIndex).Take(pageSize);
            var ret = await q.ToListAsync();
            List<T> result = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ret));
            return result;
        }


        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }
        public IEnumerable<T> Find<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> selected)
        {
            var q = Context.Set<TEntity>().Where(predicate).Select(selected);
            var ret = q.ToList();
            var result = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ret));
            return result;
        }
        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int skip, int pageSize)
        {
            return Context.Set<TEntity>().Where(predicate).Skip(skip).Take(pageSize).ToListAsync();
        }
        public async Task<List<T>> FindAsync<T>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> selected)
        {
            var q = Context.Set<TEntity>().Where(predicate).Select(selected);
            var ret = await q.ToListAsync();
            var result = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(ret));
            return result;

        }
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> Include)
        {
            return Context.Set<TEntity>().Include(Include).SingleOrDefaultAsync(predicate);
        }
        public Task<TEntity> SingleOrDefaultAsync(
    Expression<Func<TEntity, bool>> predicate,
    params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Context.Set<TEntity>().AsQueryable();

            foreach (var include in includes)
            {
                if (include != null)
                {
                    query = query.Include(include);
                }
            }

            return query.SingleOrDefaultAsync(predicate);
        }

        public void Add(TEntity entity)
        {
            //entity.CreationDate = DateTime.Now;
            Context.Set<TEntity>().Add(entity);
        }

        public Task AddAsync(TEntity entity)
        {
            return Task.Run(() =>
            {
                Add(entity);
            });
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            var enumerable = entities.ToList();
            Context.Set<TEntity>().AddRange(enumerable);
        }

        public Task AddRangeAsync(IList<TEntity> entities)
        {
            return Task.Run(() =>
            {
                AddRange(entities);
            });
        }

        public void Remove(TEntity entity)
        {
            //entity.CreationDate = DateTime.Now;
            //entity.IsDeleted = true;
            Context.Set<TEntity>().Remove(entity);
        }

        public Task RemoveAsync(TEntity entity)
        {
            return Task.Run(() =>
            {
                Remove(entity);
            });
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            //var deletedTime = DateTime.Now;
            var enumerable = entities.ToList();
            //foreach (var entity in enumerable)
            //{
                //entity.CreationDate = deletedTime;
                //entity.IsDeleted = true;                
            //}
            Context.Set<TEntity>().RemoveRange(enumerable);
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            return Task.Run(() =>
            {
                RemoveRange(entities);
            });
        }
        public void Update(TEntity entity) // update entity
        {
            //entity.ModificationDate = DateTime.Now;
            Context.Attach(entity);
            //var dt = Context.Find<TEntity>(Id);
            //CopyPropertiesTo(entity, dt);
            Context.Entry(entity).State = EntityState.Modified;
        }
        public Task UpdateAsync(TEntity entity)
        {
            return Task.Run(() =>
            {
                Update(entity);
            });

        }
        public Task<int> CountAllItems()
        {
            return Context.Set<TEntity>().CountAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllQueryable(Expression<Func<TEntity, bool>> predicate)
        {
            var Base = Context.Set<TEntity>().Where(predicate);
            return Base;
        }

        public async Task<List<TEntity>> ToListAsync(IQueryable<TEntity> query)
        {
            return await query.ToListAsync();
        }

        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> Include)
        {
            return Context.Set<TEntity>().Where(predicate).Include(Include).ToListAsync();
        }

        public Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate , params Expression<Func<TEntity, object>>[] entityInclude)
        {
            var Base = Context.Set<TEntity>().AsQueryable();
            Base = Base.Where(predicate);
            if (entityInclude != null)
            {
                foreach (var inc in entityInclude)
                {
                    Base = Base.Include(inc);
                }
            }
            return Base.ToListAsync();
        }

        public  IQueryable<TEntity> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate)
        {
            var Base = Context.Set<TEntity>().AsQueryable();
           
                Base = Base/*.Where(i => !i.IsDeleted)*/.Where(predicate);
           
            

            return Base;
        }


        public async Task<IQueryable<TEntity>> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate, bool preInclude, params Expression<Func<TEntity, object>>[] entityInclude)
        {
            var Base = Context.Set<TEntity>().AsQueryable();
            if (preInclude)
            {
                foreach (var inc in entityInclude)
                {
                    Base = Base.Include(inc);
                }

                Base = Base/*.Where(i => !i.IsDeleted)*/.Where(predicate);
            }
            else
            {
                Base = Base/*.Where(i => !i.IsDeleted)*/.Where(predicate);
                if (entityInclude != null)
                {
                    foreach (var inc in entityInclude)
                    {
                        Base = Base.Include(inc);
                    }
                }
            }

            return Base;
        }
        public async Task<IQueryable<TEntity>> FindAllAsyncQuery(Expression<Func<TEntity, bool>> predicate, bool preInclude, int skip, int take, params Expression<Func<TEntity, object>>[] entityInclude)
        {
            var Base = Context.Set<TEntity>().AsQueryable();
            if (preInclude)
            {
                foreach (var inc in entityInclude)
                {
                    Base = Base.Include(inc);
                }

                Base = Base/*.Where(i => !i.IsDeleted)*/.Where(predicate).Skip(skip).Take(take);
            }
            else
            {
                Base = Base/*.Where(i => !i.IsDeleted)*/.Where(predicate).Skip(skip).Take(take);
                if (entityInclude != null)
                {
                    foreach (var inc in entityInclude)
                    {
                        Base = Base.Include(inc);
                    }
                }
            }

            return Base;
        }
    }
}