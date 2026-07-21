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
 
    public class ReturnOrdersStatusProfile : Profile
    {
        private IMethods _methods;
        public ReturnOrdersStatusProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<OrderStatusDto, ReturnsOrderStatus>();
             

        }
        private void QueryMapper()
        {
            CreateMap<ReturnsOrderStatus, OrderStatusDto>()
                 .ForMember(dest => dest.Status, map => map.MapFrom(src => src.StatusId))
                 .ForMember(dest => dest.OrderId, map => map.MapFrom(src => src.ReturnsOrderId))
            .ForMember(dest => dest.OrderStatusId, map => map.MapFrom(src => src.ReturnsOrderStatusId))
            .ForMember(dest => dest.StatusId, map => map.MapFrom(src => src.StatusId)); ;
        }
    }

}
