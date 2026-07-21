using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.DTOs.CatType;
using Kader.DTOs.UsersStores;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Services.Utilities.Mappers;
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity; 
namespace Kader.Services.Implementations.UsersStores
{

    public class UsersStoresService : IUsersStoresService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UsersStoresService(IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _unitOfWork = unitOfWork;
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }

        public async Task<ReturnDto<List<UsersStoresDto>>> GetUsersStores()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.UsersStores>? getData = await _unitOfWork.UsersStores.FindAsync(h => h.IsDeleted == false) ?? new List<Data.DataAccessLayer.Entities.UsersStores>();
                var map = _mapper.Map<List<UsersStoresDto>>(getData);
                return new ReturnDto<List<UsersStoresDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<UsersStoresDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> UpdateUserStoresAsync(HttpContext context, List<int> storeIds, string userId)
        {
            try
            {
                var currentUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                // Get existing store assignments for the user
                var existingUserStores = await _unitOfWork.UsersStores
                    .FindAsync(us => us.UserId == userId);

                var existingStoreIds = existingUserStores.Select(us => us.StoreId).ToList();

                // Stores to remove (no longer assigned)
                var storesToRemove = existingUserStores.Where(us => !storeIds.Contains(us.StoreId.Value)).ToList();
                _unitOfWork.UsersStores.RemoveRange(storesToRemove);

                // Stores to add (newly assigned)
                var newStoresToAdd = storeIds
                    .Where(storeId => !existingStoreIds.Contains(storeId))
                    .Select(storeId => new Data.DataAccessLayer.Entities.UsersStores
                    {
                        InsertedBy = currentUserId,
                        UserId = userId,
                        StoreId = storeId
                    }).ToList();

                if (newStoresToAdd.Any())
                {
                    await _unitOfWork.UsersStores.AddRangeAsync(newStoresToAdd);
                }
                if (newStoresToAdd.Count()==0&& existingUserStores.Count() > 0)
                {
                    return new ReturnDto<bool>(true, true, "تم تحديث بنجاح.");
                }
                // Save changes
                if (await _unitOfWork.CompleteAsync() > 0)
                {
                    return new ReturnDto<bool>(true, true, "تم تحديث المخازن بنجاح.");
                }
                else
                {
                    return new ReturnDto<bool>(false, false, "لم يتم التحديث، حدث خطأ!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<List<UsersStoresDto>>> GetDeletedUsersStores()
        {
            try
            {
                List<Data.DataAccessLayer.Entities.UsersStores>? getData = await _unitOfWork.UsersStores.FindAsync(h => h.IsDeleted == true) ?? new List<Data.DataAccessLayer.Entities.UsersStores>();
                var map = _mapper.Map<List<UsersStoresDto>>(getData);
                return new ReturnDto<List<UsersStoresDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<UsersStoresDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> SaveUsersStores(HttpContext context, List<int> UsersStoresList, string userid)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                var usersStoresEntities = UsersStoresList.Select(storeId => new Data.DataAccessLayer.Entities.UsersStores
                {
                    InsertedBy = stringUserId,
                    UserId = userid,
                    StoreId = storeId,
                    //Shipinng = roleType == "Shipping",
                    //Storing = roleType == "Storing"
                }).ToList();

                // Bulk insert
                await _unitOfWork.UsersStores.AddRangeAsync(usersStoresEntities);


                if (await _unitOfWork.CompleteAsync() > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(false, false, "Not Saved, Error Occurred !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<List<UsersStoresDto>>> GetUserStores(string userId)
        {
            var getData = await _unitOfWork.UsersStores.FindAsync(us => us.UserId == userId, i => i.Store);
            if (getData == null) return new ReturnDto<List<UsersStoresDto>>(false, null, "Nothing found!");

            var map = _mapper.Map<List<UsersStoresDto>>(getData);
            return new ReturnDto<List<UsersStoresDto>>(true, map, string.Empty);
        }
        public async Task<ReturnDto<List<UsersStoresDto>>> GetAllUserStores(int StoreId)
        {
            var getData = await _unitOfWork.UsersStores.FindAsync(h => (h.StoreId == StoreId || StoreId == 0)&& (h.IsDeleted == false|| h.IsDeleted == null), i => i.Store, i => i.User);
            if (getData == null) return new ReturnDto<List<UsersStoresDto>>(false, null, "Nothing found!");

            var map = _mapper.Map<List<UsersStoresDto>>(getData);
            return new ReturnDto<List<UsersStoresDto>>(true, map, string.Empty);
        }
         
        public async Task<ReturnDto<bool>> DeleteUsersStores(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.UsersStores.SingleOrDefaultAsync(l => l.StoreId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.UsersStores.UpdateAsync(newCat);

                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> RestoreDeleteUsersStores(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.UsersStores.SingleOrDefaultAsync(l => l.StoreId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.UsersStores.UpdateAsync(newCat);

                var result = _unitOfWork.Complete();
                if (result > 0)
                    return new ReturnDto<bool>(true, true, string.Empty);
                else
                    return new ReturnDto<bool>(true, false, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<bool>(false, false, ex.Message);
            }
        }

        public async Task<ReturnDto<UsersStoresDto>> GetSingleUsersStores(int id)
        {
            try
            {
                var getData = await _unitOfWork.UsersStores.SingleOrDefaultAsync(l => l.IsDeleted == false && l.StoreId == id);
                if (getData == null) return new ReturnDto<UsersStoresDto>(false, null, "Nothing found!");

                var map = _mapper.Map<UsersStoresDto>(getData);
                return new ReturnDto<UsersStoresDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<UsersStoresDto>(false, null, ex.Message);
            }
        }

    }
}
