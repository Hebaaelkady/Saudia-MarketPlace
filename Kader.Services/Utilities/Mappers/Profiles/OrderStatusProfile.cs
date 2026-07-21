using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.CatType;
using Kader.DTOs.Colors;
using Kader.DTOs.Product;
using Kader.DTOs.OrderStatus;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class OrderStatusProfile : Profile
    {
        private IMethods _methods;
        public OrderStatusProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<OrderStatusDto, OrderStatus>();
             

        }
        private void QueryMapper()
        {
            CreateMap<OrderStatus, OrderStatusDto>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName));
            
        }
    }

}
