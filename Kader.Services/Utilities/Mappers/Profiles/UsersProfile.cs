using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Users;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class UsersProfile : Profile
    {
        private IMethods _methods;
        public UsersProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            //CreateMap<LoginDto, AspNetUsers>();
           CreateMap<AllUserViewDto, AspNetUsers>();
            CreateMap<Register_backendDto, AllUserViewDto>();
            CreateMap<AllUserViewDto, ApplicationUser>();
        }
        private void QueryMapper()
        {
            CreateMap<AllUserViewDto, Register_backendDto>();
            CreateMap<ApplicationUser, UserPhoneDto>();
            CreateMap<ApplicationUser, AllUserViewDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Ignore the password property
            ;
            CreateMap<AspNetUsers, AllUserViewDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
            .ForMember(dest => dest.Password, opt => opt.Ignore()) // Ignore the password property
            ;
        }
    }

}
