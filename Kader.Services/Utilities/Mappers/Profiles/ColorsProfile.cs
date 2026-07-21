using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Catogry;
using Kader.DTOs.Colors; 
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class ColorsProfile : Profile
    {
        private IMethods _methods;
        public ColorsProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<ColorsDto, Colors>();

            CreateMap<Colors_backendDto, Colors>();

        }
        private void QueryMapper()
        {
            CreateMap<Colors, ColorsDto>() ;
            
        }
    }

}
