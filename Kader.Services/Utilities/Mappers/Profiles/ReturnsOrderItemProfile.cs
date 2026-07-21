using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.ReturnsOrderItem;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.Cart;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class ReturnsOrderItemProfile : Profile
    {
        private IMethods _methods;
        public ReturnsOrderItemProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<ReturnsOrderItemDto, ReturnsOrderItem>();

        }
        private void QueryMapper()
        {
            CreateMap<ReturnsOrderItem, ReturnsOrderItemDto>();
            CreateMap<ReturnsOrderItem, IncreaseReturnsOrderItemDto>()
                .ForMember(dest => dest.ProductId, map => map.MapFrom(src => src.OrderItems.ProductId))
                .ForMember(dest => dest.Quantity, map => map.MapFrom(src => src.OrderItems.Quantity))
                .ForMember(dest => dest.ReturnsOrderId, map => map.MapFrom(src => src.ReturnsOrderId));
            CreateMap<ReturnsOrderItem, ReturnsOrderItemslistDto>()
                .ForMember(dest => dest.ProductName, map => map.MapFrom(src => src.OrderItems.ProductName))
                .ForMember(dest => dest.Quantity, map => map.MapFrom(src => src.OrderItems.Quantity))
                   .ForMember(dis => dis.PriceItem, map => map.MapFrom(sourse => sourse.OrderItems.PriceItem))
             .ForMember(dis => dis.ShippingPrice, map => map.MapFrom(sourse => sourse.OrderItems.ShippingPrice));

            CreateMap<ReturnsOrderItem, ReturnsOrderItemsDetailDto>()
                 .ForMember(dest => dest.unit, map => map.MapFrom(src => src.OrderItems.Product.UnitName))
                .ForMember(dest => dest.ProductId, map => map.MapFrom(src => src.OrderItems.ProductId))
                    .ForMember(dest => dest.ProductName, map => map.MapFrom(src => src.OrderItems.ProductName))
                    .ForMember(dest => dest.Quantity, map => map.MapFrom(src => src.OrderItems.Quantity))
                       .ForMember(dis => dis.PriceItem, map => map.MapFrom(sourse => sourse.OrderItems.PriceItem))
                       .ForMember(dis => dis.Reason, map => map.MapFrom(sourse => sourse.ReasonNavigation.ReasonName))
                        .ForMember(dis => dis.sku, map => map.MapFrom(sourse => sourse.OrderItems.Product.Barcode))
                 .ForMember(dis => dis.ShippingPrice, map => map.MapFrom(sourse => sourse.OrderItems.ShippingPrice));
            CreateMap<ReturnsOrderItem, ReturnsItemsDto>()
              .ForMember(dest => dest.ProductId, map => map.MapFrom(src => src.OrderItems.ProductId))
                  .ForMember(dest => dest.ProductName, map => map.MapFrom(src => src.OrderItems.ProductName))
                  .ForMember(dest => dest.Quantity, map => map.MapFrom(src => src.OrderItems.Quantity))
                     .ForMember(dis => dis.PriceItem, map => map.MapFrom(sourse => sourse.OrderItems.PriceItem))
                      ;

        }
    }

}
