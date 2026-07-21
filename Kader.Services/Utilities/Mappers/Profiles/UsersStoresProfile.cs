using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Stores;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.DTOs.UsersStores;

namespace Kader.Services.Utilities.Mappers.Profiles
{

    public class UsersStoresProfile : Profile
    {
        private IMethods _methods;
        public UsersStoresProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<UsersStoresDto, UsersStores>();

        }
        private void QueryMapper()
        {
            CreateMap<UsersStores, UsersStoresDto>().ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                  .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                  .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.User.CountryCode))
                   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber));
        }

    }
}
