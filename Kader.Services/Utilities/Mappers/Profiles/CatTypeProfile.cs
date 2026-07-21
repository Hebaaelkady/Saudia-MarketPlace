using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.CatType;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class CatTypeProfile : Profile
    {
        private IMethods _methods;
        public CatTypeProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<CatTypeDto, CatType>(); 

        }
        private void QueryMapper()
        {
            CreateMap<CatType, CatTypeDto>() ;
            
        }
    }

}
