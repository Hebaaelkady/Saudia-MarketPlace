using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kader.Data.DataAccessLayer.Repositories
{
    public class Repository_SP<TEntity>:IRepository_SP<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository_SP(DbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetSingle(string storedProcedure, params string[] orderedParameters)
        {
            string concatParams = string.Empty;
            int index = 0;
            foreach (var item in orderedParameters)
            {
                string add = string.Empty;
                index++;
                if (index < orderedParameters.Length)
                    add = ", ";
                if (item != "NULL")
                    concatParams += " N'" + item + "'" + add;
                else
                    concatParams += " " + item + add;
            }
            var result = Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").SingleOrDefaultAsync();
            return await result;
            //return await Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").SingleOrDefaultAsync();
        }
        public IQueryable<TEntity> GetListTest1(string storedProcedure, params string[] orderedParameters)
        {
            try
            {
                string concatParams = string.Empty;
                int index = 0;
                foreach (var item in orderedParameters)
                {
                    string add = string.Empty;
                    index++;
                    if (index < orderedParameters.Length)
                        add = ", ";
                    if (item != "NULL")
                        concatParams += " N'" + item + "'" + add;
                    else
                        concatParams += " " + item + add;
                }
                //  concatParams = " N'101',  N'0',  '10105102,10105103',  NULL,  NULL,  NULL";
                var result = Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").AsQueryable();
                return result;
            }

            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public  List<TEntity> GetListTest(string storedProcedure, params string[] orderedParameters)
        {
            try
            {
                string concatParams = string.Empty;
                int index = 0;
                foreach (var item in orderedParameters)
                {
                    string add = string.Empty;
                    index++;
                    if (index < orderedParameters.Length)
                        add = ", ";
                    if (item != "NULL")
                        concatParams += " N'" + item + "'" + add;
                    else
                        concatParams += " " + item + add;
                }
                //  concatParams = " N'101',  N'0',  '10105102,10105103',  NULL,  NULL,  NULL";
                var result =  Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").ToList();
                return result;
            }

            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public  int CountList(string storedProcedure, params string[] orderedParameters)
        {
            try
            {
                string concatParams = string.Empty;
                int index = 0;
                foreach (var item in orderedParameters)
                {
                    string add = string.Empty;
                    index++;
                    if (index < orderedParameters.Length)
                        add = ", ";
                    if (item != "NULL")
                        concatParams += " N'" + item + "'" + add;
                    else
                        concatParams += " " + item + add;
                }
                //  concatParams = " N'101',  N'0',  '10105102,10105103',  NULL,  NULL,  NULL";
                var result = Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").AsEnumerable();
                var result1 =result.Count();
                return   result1;
            }

            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<List<TEntity>> GetList(string storedProcedure, params string[] orderedParameters)
        {
            try
            {
                string concatParams = string.Empty;
                int index = 0;
                foreach (var item in orderedParameters)
                {
                    string add = string.Empty;
                    index++;
                    if (index < orderedParameters.Length)
                        add = ", ";
                    if(item != "NULL" )
                        concatParams += " N'" + item + "'" + add;
                    else
                        concatParams += " " + item + add;
                }
              //  concatParams = " N'101',  N'0',  '10105102,10105103',  NULL,  NULL,  NULL";
                var result = await Context.Set<TEntity>().FromSqlRaw($"{storedProcedure} {concatParams}").ToListAsync();
                return result;
            }

            catch (DbException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}