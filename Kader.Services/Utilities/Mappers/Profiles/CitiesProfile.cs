using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Cities;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class CitiesProfile : Profile
    {
        private IMethods _methods;
        public CitiesProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<CitiesDto, Cities>(); 

        }
        private void QueryMapper()
        {
            CreateMap<Cities, CitiesDto>() ;
            
        }
    }

}
