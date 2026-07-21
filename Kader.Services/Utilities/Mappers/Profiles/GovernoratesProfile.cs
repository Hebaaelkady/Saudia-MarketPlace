using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Governorates;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class GovernoratesProfile : Profile
    {
        private IMethods _methods;
        public GovernoratesProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<GovernoratesDto, Governorates>(); 

        }
        private void QueryMapper()
        {
            CreateMap<Governorates, GovernoratesDto>() ;
            
        }
    }

}
