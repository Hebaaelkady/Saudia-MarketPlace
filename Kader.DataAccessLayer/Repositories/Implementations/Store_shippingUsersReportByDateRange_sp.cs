using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{ 
     
        public class Store_shippingUsersReportByDateRange_sp : Repository_SP<Store_shippingUsersReportByDateRange>, IStore_shippingUsersReportByDateRange_sp
    {
        private DBContext context; private readonly DBContext _dbContext;

        
        public Store_shippingUsersReportByDateRange_sp(DBContext context) : base(context)
        {
            _dbContext = context;  // ✅ Assign the passed DbContext
        }


      

        public async Task<List<Store_shippingUsersReportByDateRange>> GetList(params object[] orderedParameters)
        {
            try
            {
                 
                if (orderedParameters.Length != 2)
                {
                     
                    return null;
                }

                var result = await _dbContext.Store_shippingUsersReportByDateRange
                    .FromSqlRaw("EXEC [egyhubc2_jina].[Store_shippingUsersReportByDateRange] @p0, @p1", orderedParameters[0], orderedParameters[1])
                    .ToListAsync();

                if (result == null || !result.Any())
                {
                    Console.WriteLine("❌ Stored procedure returned no data.");
                }
                else
                {
                    Console.WriteLine($"✅ Stored procedure returned {result.Count} records.");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR executing stored procedure: {ex.Message}");
                return null;
            }
        }



        public async Task<Store_shippingUsersReportByDateRange> GetSingle(params string[] orderedParameters)
        {
            return await GetSingle("Store_shippingUsersReportByDateRange", orderedParameters);
        }
    }
}
