using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Governorates;
using Kader.DTOs.Neighborhood;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class NeighborhoodProfile : Profile
    {
        private IMethods _methods;
        public NeighborhoodProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<NeighborhoodDto, Neighborhood>(); 

        }
        private void QueryMapper()
        {
            CreateMap<Neighborhood, NeighborhoodDto>() ;
            
        }
    }

}
