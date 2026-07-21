using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.CatType;
using Kader.DTOs.UsersStores;
using Kader.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.UsersStores
{

    public interface IUsersStoresService
    {
        Task<ReturnDto<bool>> UpdateUserStoresAsync(HttpContext context, List<int> storeIds, string userId);
        Task<ReturnDto<List<UsersStoresDto>>> GetUsersStores();
        Task<ReturnDto<bool>> SaveUsersStores(HttpContext context, List<int> UsersStoresList,string userid );
        Task<ReturnDto<bool>> DeleteUsersStores(HttpContext context, int id);
        Task<ReturnDto<UsersStoresDto>> GetSingleUsersStores(int id);
        Task<ReturnDto<List<UsersStoresDto>>> GetDeletedUsersStores();
        Task<ReturnDto<bool>> RestoreDeleteUsersStores(HttpContext context, int id);
        Task<ReturnDto<List<UsersStoresDto>>> GetUserStores(string userId);
        Task<ReturnDto<List<UsersStoresDto>>> GetAllUserStores(int StoreId); 
    }
}
