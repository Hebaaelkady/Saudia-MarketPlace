using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Catogry;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class CatogryProfile : Profile
    {
        private IMethods _methods;
        public CatogryProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<CatogryDto, Catogry>();
            CreateMap<Catogry_backendDto, Catogry>();

        }
        private void QueryMapper()
        {
            CreateMap<Catogry, CatogryDto>().ForMember(dis => dis.CatName, map => map.MapFrom(sourse => sourse.Cat.CatogryName)); ;
            CreateMap<Catogry, JsTreeModelDto>() ;
            CreateMap<Catogry, ApiCatogryDto > ()
                .ForMember(dis => dis.nameTy, map => map.MapFrom(sourse => sourse.CatogryName))
                .ForMember(dis => dis.CatTypeID, map => map.MapFrom(sourse => sourse.TypeId))
               .ForMember(dis => dis.id, map => map.MapFrom(sourse => sourse.CatogryId)); ;
            CreateMap<ApiCatogryDto, CatogryDto>().ForMember(dis => dis.CatName, map => map.MapFrom(sourse => sourse.name))
                .ForMember(dis => dis.CatogryId, map => map.MapFrom(sourse => sourse.id)); ;

        }
    }

}
