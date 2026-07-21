using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.CatType;
using Kader.DTOs.Colors;
using Kader.DTOs.Product;
using Kader.DTOs.Units;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class UnitsProfile : Profile
    {
        private IMethods _methods;
        public UnitsProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<UnitsDto, Units>();

            CreateMap<Units_backendDto, Units>();

        }
        private void QueryMapper()
        {
            CreateMap<Units, UnitsDto>() ;
            
        }
    }

}
