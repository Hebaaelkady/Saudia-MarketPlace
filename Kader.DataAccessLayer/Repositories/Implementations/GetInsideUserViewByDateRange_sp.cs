using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.Data.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kader.Data.DataAccessLayer.Repositories.Implementations
{ 
     
        public class GetInsideUserViewByDateRange_sp : Repository_SP<GetInsideUserViewByDateRange>, IGetInsideUserViewByDateRange_sp
    {
        private DBContext context; private readonly DBContext _dbContext;

        
        public GetInsideUserViewByDateRange_sp(DBContext context) : base(context)
        {
            _dbContext = context;  // ✅ Assign the passed DbContext
        }


      

        public async Task<List<GetInsideUserViewByDateRange>> GetList(params object[] orderedParameters)
        {
            try
            {
                Console.WriteLine($"Executing SP: GetOrdersByDateRange with parameters: {string.Join(", ", orderedParameters)}");

                // Ensure there are exactly two parameters (StartDate and EndDate)
                if (orderedParameters.Length != 2)
                {
                    Console.WriteLine("❌ Error: Incorrect number of parameters. Expected 2 (StartDate, EndDate).");
                    return null;
                }

                var result = await _dbContext.GetInsideUserViewByDateRange
                    .FromSqlRaw("EXEC [egyhubc2_jina].[GetOrdersByDateRange] @p0, @p1", orderedParameters[0], orderedParameters[1])
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



        public async Task<GetInsideUserViewByDateRange> GetSingle(params string[] orderedParameters)
        {
            return await GetSingle("GetInsideUserViewByDateRange", orderedParameters);
        }
    }
}
