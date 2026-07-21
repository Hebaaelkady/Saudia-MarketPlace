using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Address;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class AddressProfile : Profile
    {
        private IMethods _methods;
        public AddressProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<AddressDto, Address>();

        }
        private void QueryMapper()
        {
            CreateMap<Address, AddressDto>().ForMember(dis => dis.CityName, map => map.MapFrom(sourse => sourse.CityNavigation.CityName))
            .ForMember(dis => dis.Name, map => map.MapFrom(sourse => sourse.Neighborhood.Name))
            .ForMember(dis => dis.Governoratesname, map => map.MapFrom(sourse => sourse.Governorates.GovernorateName)); 
        }
    }

}
