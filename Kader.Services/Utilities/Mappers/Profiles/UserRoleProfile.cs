using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Roles;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class UserRoleProfile : Profile
    {
        private IMethods _methods;
        public UserRoleProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<RolesDto, ApplicationRole>();
            //CreateMap<LoginUserDto, AspNetUsers>();

        }
        private void QueryMapper()
        {
            CreateMap<ApplicationRole, RolesDto>();
            //CreateMap<AspNetUsers, LoginUserDto>();

        }
    }

}
