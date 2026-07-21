using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs;
using Kader.DTOs.Catogry;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.Catogry
{

    public interface ICatogryService
    {
        Task<ReturnDto<List<T>>> GetCatogry<T>(HttpContext context, int? type) where T : class, new();
        Task<ReturnDto<CatogryDto>> GetSingleCatogry(HttpContext context, int id);
        Task<ReturnDto<CatogryDto>> GetSingleCatogry(string id);
        Task<ReturnDto<int>> SaveCatogry(HttpContext context, CatogryDto Cat);
        Task<ReturnDto<List<CatogryDto>>> GetSubCatogry(HttpContext context, int? type ); 
        List<JsTreeModelDto> GetTreeNodes(HttpContext context, string parentId, int? type, List<int> GetCatogryByRole);
        Task<ReturnDto<bool>> DeleteCatogry(HttpContext context, int id);
        Task<ReturnDto<bool>> DeleteAllSubCatogry(HttpContext context,  List<int> ids);
        Task<List<int>> RecursiveGetllSubCategories(List<int> categoryIds);
        Task<ReturnDto<List<int>>> GetCatogryByRole(HttpContext context, int? type);
        Task<ReturnDto<List<CatogryDto>>> GetDeletedCatogry(HttpContext context, int? CatId, int? type);
        Task<ReturnDto<bool>> RestoreDeleteCatogry(HttpContext context, int id);
        List<JsTreeModelDto> GetTreeNodes(HttpContext context, string parentId, int? type);

        List<JsTreeModelDto> GetTreeNodesHasProduct(HttpContext context, string parentId, int? type);

        Task<ReturnDto<List<ApiCatogryDto>>> GetItemInfoAsync(HttpContext context, int type, string token);
    }
}
