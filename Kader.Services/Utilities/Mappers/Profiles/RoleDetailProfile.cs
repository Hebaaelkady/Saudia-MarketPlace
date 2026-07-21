using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.CatType;
using Kader.DTOs.Product;
using Kader.DTOs.RoleDetail;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class RoleDetailProfile : Profile
    {
        private IMethods _methods;
        public RoleDetailProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<RoleDetailDto, RoleDetail>(); 

        }
        private void QueryMapper()
        {
            CreateMap<RoleDetail, RoleDetailDto>() ;
            
        }
    }

}
