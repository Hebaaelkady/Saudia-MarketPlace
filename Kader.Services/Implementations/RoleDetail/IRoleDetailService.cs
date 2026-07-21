using System.Collections.Generic;
using System.Threading.Tasks;
using Kader.DTOs; 
using Kader.DTOs.RoleDetail;
using Microsoft.AspNetCore.Http;

namespace Kader.Services.Implementations.RoleDetail
{

    public interface IRoleDetailService
    {
        Task<ReturnDto<List<RoleDetailDto>>> GetRoleDetail();
        Task<ReturnDto<bool>> SaveRoleDetail(HttpContext context, IList<RoleDetailDto> RoleDetailDto);
        Task<ReturnDto<bool>> DeleteRoleDetail(HttpContext context, string id);
    }
}
