using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.ShippingPrice;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class ShippingPriceProfile : Profile
    {
        private IMethods _methods;
        public ShippingPriceProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<ShippingPriceDto, ShippingPrice>(); 

        }
        private void QueryMapper()
        {
            CreateMap<ShippingPrice, ShippingPriceDto>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsODD_Even, opt => opt.MapFrom(src => src.IsOddEven)); ;

        }
    }

}
