using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Kader.Data.DataAccessLayer;
using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.Services.Utilities.Mappers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
 
namespace Kader.Services.Implementations.Catogry
{

    public class CatogryService : ICatogryService
    {
        private readonly HttpClient _httpClient;
        private IUnitOfWork _unitOfWork; private IKeys _keys;
        private IMapper _mapper; private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public CatogryService(HttpClient httpClient, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, RoleManager<ApplicationRole> roleManage)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork; _userManager = userManager; _keys = new Keys();
            _mapper = ObjectMapper.Mapper; _roleManager = roleManage;
        }
        public async Task<ReturnDto<CatogryDto>> GetSingleCatogry(HttpContext context,int id)
        {
            try
            {
                var getData = await _unitOfWork.Catogry.SingleOrDefaultAsync(l => l.IsDeleted == false && l.CatogryId == id);
                if (getData == null) return new ReturnDto<CatogryDto>(false, null, "Nothing found!");
                var map = _mapper.Map<CatogryDto>(getData);
                return new ReturnDto<CatogryDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<CatogryDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<CatogryDto>> GetSingleCatogry( string id)
        {
            try
            {
                var getData = await _unitOfWork.Catogry.SingleOrDefaultAsync(l => l.IsDeleted == false && l.CatogryName == id);
                if (getData == null) return new ReturnDto<CatogryDto>(false, null, "Nothing found!");

                var map = _mapper.Map<CatogryDto>(getData);
                return new ReturnDto<CatogryDto>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Selocs.System.Error: {ex.Message}");
                return new ReturnDto<CatogryDto>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<T>>> GetCatogry<T>(HttpContext context, int? type) where T : class, new()

        {
            //var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            try
            {
                List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && o.TypeId == type  ) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
                var map = _mapper.Map<List<T>>(getData);
                return new ReturnDto<List<T>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<T>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<CatogryDto>>> GetDeletedCatogry(HttpContext context, int? CatId, int? type)

        {
            try
            {
                List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == true && o.TypeId == type && o.CatId == CatId) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
                var map = _mapper.Map<List<CatogryDto>>(getData);
                return new ReturnDto<List<CatogryDto>>(true, map, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<CatogryDto>>(false, null, ex.Message);
            }
        }


        public async Task<List<int>> RecursiveGetllSubCategories(List<int> categoryIds)
        {
            IList<int> subCategories = new List<int>();
            foreach (var categoryId in categoryIds)
            {
                var getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false&& (o.CatId == categoryId));
                if (getData.Any()) // base case: if there are no more subcategories, stop the recursion
                {
                    var subCategoryIds = getData.Select(l => l.CatogryId).ToList();
                    var message = await RecursiveGetllSubCategories(subCategoryIds);
                    subCategories = subCategories.Concat(message).ToList(); // combine the results of all recursive calls
                }
                subCategories.Add(categoryId);
            }
            return (List<int>)subCategories;
        }
        public async Task<ReturnDto<List<CatogryDto>>> GetSubCatogry(HttpContext context, int? type )
        {
            //var CatogryID = context.User.Claims.FirstOrDefault(c => c.Type == "CatogryID")?.Value;
            //var TypeID = context.User.Claims.FirstOrDefault(c => c.Type == "TypeID")?.Value;
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            var actIDpiCattypeList = context.User.Claims
    .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)) // Filter out null or empty values
    .Select(c =>
    {
        if (int.TryParse(c.Value, out var value)) // Safely parse the value
        {
            return (int?)value; // Return a nullable int
        }
        return null; // Return null for invalid values
    })
    .Where(v => v.HasValue) // Exclude null values
    .Select(v => v.Value)  // Convert back to non-nullable ints
    .ToList();
            try
            {
                if (isAdmin|| roleClaims.Contains(_keys.ProductRole()) || roleClaims.Contains(_keys.CatogryRole()))
                {
                    List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && o.TypeId == type, i2 => i2.Cat) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
                    var map = _mapper.Map<List<CatogryDto>>(getData);
                    return new ReturnDto<List<CatogryDto>>(true, map, string.Empty);
                }
                else
                {
                  //  List<int> myList = new List<int> { int.Parse(CatogryID) };
                    //var message = await RecursiveGetllSubCategories(GetCatogryByRole);
                    List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && o.TypeId == type && actIDpiCattypeList.Any(id => id == o.CatogryId), i2 => i2.Cat) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
                    var map = _mapper.Map<List<CatogryDto>>(getData);
                    return new ReturnDto<List<CatogryDto>>(true, map, string.Empty);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<CatogryDto>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<List<int>>> GetCatogryByRole(HttpContext context,int? type)
        
        {
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            try
            {   
                List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && o.TypeId==type&& (roleClaims.Any(y => y == o.RoleId)), i2 => i2.Cat) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
               if(getData != null)
                {
                   // var map = _mapper.Map<List<int>>(getData);
                    return new ReturnDto<List<int>>(true, getData.Select(l=>l.CatogryId).ToList(), string.Empty);
                }
               else
                    return new ReturnDto<List<int>>(false, null,"not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<List<int>>(false, null, ex.Message);
            }
        }
        public async Task<ReturnDto<int>> SaveCatogry(HttpContext context, CatogryDto Cat)
        {
            try
            { 
             var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
              
            if (Cat.CatogryId == 0)
                { 
                    var newCat = _mapper.Map<Data.DataAccessLayer.Entities.Catogry>(Cat);
                newCat.InsertedBy = stringUserId;
                await _unitOfWork.Catogry.AddAsync(newCat);
                    if (_unitOfWork.Complete() > 0)
                        return new ReturnDto<int>(true, newCat.CatogryId, string.Empty);
                    else
                        return new ReturnDto<int>(false, 0, "Not Saved, Error Occurred !");
                }
                else
                {
                    var oldCat = await _unitOfWork.Catogry.GetAsync(Cat.CatogryId);
                    var newCat = _mapper.Map(Cat, oldCat);
                newCat.UpdateDate = DateTime.UtcNow.AddHours(3);
                newCat.UpdateBy = stringUserId;
                await _unitOfWork.Catogry.UpdateAsync(newCat);
                    if (_unitOfWork.Complete() > 0)
                        return new ReturnDto<int>(true, newCat.CatogryId, string.Empty);
                    else
                        return new ReturnDto<int>(false, 0, "Not Saved, Error Occurred !");
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Kader.System.Error: {ex.Message}");
                return new ReturnDto<int>(false, 0, ex.Message);
            }
        }
        public async Task<ReturnDto<bool>> RestoreDeleteCatogry(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Catogry.SingleOrDefaultAsync(l => l.CatogryId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = false;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Catogry.UpdateAsync(newCat);

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
        public async Task<ReturnDto<bool>> DeleteCatogry(HttpContext context, int id)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var newCat = await _unitOfWork.Catogry.SingleOrDefaultAsync(l => l.CatogryId == id);
                newCat.DeletedBy = stringUserId;
                newCat.IsDeleted = true;
                newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                await _unitOfWork.Catogry.UpdateAsync(newCat);

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
        public async Task<ReturnDto<bool>> DeleteAllSubCatogry(HttpContext context, List<int> ids)
        {
            try
            {
                var stringUserId = context.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
               // List<Data.DataAccessLayer.Entities.Catogry> getData = await _unitOfWork.Catogry.FindAsync(o => o.IsDeleted == false && o.CatId == id) ?? new List<Data.DataAccessLayer.Entities.Catogry>();
                foreach (var id in ids)
                {
                    var newCat = await _unitOfWork.Catogry.SingleOrDefaultAsync(l => l.CatogryId == id);
                    newCat.DeletedBy = stringUserId;
                    newCat.IsDeleted = true;
                    newCat.DeletedDate = DateTime.UtcNow.AddHours(3);
                    await _unitOfWork.Catogry.UpdateAsync(newCat);
                } 
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


        public List<JsTreeModelDto> GetTreeNodes(HttpContext context, string parentId,int? type,List<int> GetCatogryByRole)
        {
            var roleClaims = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
            List<JsTreeModelDto> nodes = null;

            if (isAdmin)
            {
                var getCatogry = _unitOfWork.Catogry.FindAsync(f => f.IsDeleted == false).Result;
                 nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false && node.TypeId == type)
                    .Select(node => new JsTreeModelDto
                    {
                        Id = node.CatogryId,
                        ParentId = node.CatId,
                        Text = node.CatogryName,
                        Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                        Level = node.CatLevel,
                    }).OrderBy(l => l.Level).ToList();

               
            }
            else
            {
                var getCatogry = _unitOfWork.Catogry.FindAsync(f => f.IsDeleted == false).Result;
                if (parentId == null)
                {
                     nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false && GetCatogryByRole.Any(y => y == node.CatogryId))
                       .Select(node => new JsTreeModelDto
                       {
                           Id = node.CatogryId,
                           ParentId = node.CatId,
                           Text = node.CatogryName,
                           Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                           Level = node.CatLevel,
                       }).OrderBy(l => l.Level).ToList();
                }
                else
                {


                     nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false )
                        .Select(node => new JsTreeModelDto
                        {
                            Id = node.CatogryId,
                            ParentId = node.CatId,
                            Text = node.CatogryName,
                            Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                            Level = node.CatLevel,
                        }).OrderBy(l => l.Level).ToList();
                }
                
            }
            return nodes;
        } 
    
    public List<JsTreeModelDto> GetTreeNodes(HttpContext context, string parentId, int? type )
    {
          List<JsTreeModelDto> nodes = null;

         
            var getCatogry = _unitOfWork.Catogry.FindAsync(f => f.IsDeleted == false).Result;
            if (parentId == null)
            {
                nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false )
                  .Select(node => new JsTreeModelDto
                  {
                      Id = node.CatogryId,
                      ParentId = node.CatId,
                      Text = node.CatogryName,
                      Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                      Level = node.CatLevel,
                  }).OrderBy(l => l.Level).ToList();
            }
            else
            {


                nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false)
                   .Select(node => new JsTreeModelDto
                   {
                       Id = node.CatogryId,
                       ParentId = node.CatId,
                       Text = node.CatogryName,
                       Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                       Level = node.CatLevel,
                   }).OrderBy(l => l.Level).ToList();
            }

         
        return nodes;
    }


        public List<JsTreeModelDto> GetTreeNodesHasProduct(HttpContext context, string parentId, int? type)
        {
            List<JsTreeModelDto> nodes = null;


            var getCatogry = _unitOfWork.Catogry.FindAsync(f=>f.IsDeleted==false).Result;
            if (parentId == null)
            {
                nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false&&node.TypeId==type)
                  .Select(node => new JsTreeModelDto
                  {
                      Id = node.CatogryId,
                      ParentId = node.CatId,
                      Text = node.CatogryName,
                      Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                      Level = node.CatLevel,
                  }).OrderBy(l => l.Level).ToList();
            }
            else
            {


                nodes = _unitOfWork.Catogry.Find(node => node.CatId.ToString() == parentId && node.IsDeleted == false && node.TypeId == type)
                   .Select(node => new JsTreeModelDto
                   {
                       Id = node.CatogryId,
                       ParentId = node.CatId,
                       Text = node.CatogryName,
                       Children = getCatogry.Any(n => n.CatId == node.CatogryId),
                       Level = node.CatLevel,
                   }).OrderBy(l => l.Level).ToList();
            }


            return nodes;
        }
        public async Task<ReturnDto<List<ApiCatogryDto>>> GetItemInfoAsync(HttpContext context, int type, string token)
        {
            try {
                bool isAdmin = context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == _keys.Admin());
                var TypeUser = context.User.Claims.FirstOrDefault(c => c.Type == "TypeUser")?.Value;

                var actIDpiCattypeList = context.User.Claims
    .Where(c => c.Type == "Stores" && !string.IsNullOrEmpty(c.Value)) // Filter out null or empty values
    .Select(c =>
    {
        if (int.TryParse(c.Value, out var value)) // Safely parse the value
        {
            return (int?)value; // Return a nullable int
        }
        return null; // Return null for invalid values
    })
    .Where(v => v.HasValue) // Exclude null values
    .Select(v => v.Value)  // Convert back to non-nullable ints
    .ToList();

                var request = new HttpRequestMessage(HttpMethod.Get, "https://jinaapi.auditor.sa/Categories/Info");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var apiProductDtos = JsonConvert.DeserializeObject<List<ApiCatogryDto>>(jsonString);

                if (isAdmin||TypeUser==null || TypeUser == "2")
                {
                    return new ReturnDto<List<ApiCatogryDto>>(true, apiProductDtos, string.Empty);
                }
                else 
                {
                    
                    return new ReturnDto<List<ApiCatogryDto>>(true, apiProductDtos, string.Empty);
                }
                // No mapping is needed here
                // Return the deserialized ApiProductDtos directly 
               
            }
    catch (Exception ex)
    {
        Console.WriteLine($"Kader.System.Error: {ex.Message}");
        return new ReturnDto<List<ApiCatogryDto>>(false, null, ex.Message);
    }
}
    }

}
