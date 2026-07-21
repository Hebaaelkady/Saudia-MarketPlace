using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using System.Collections.Generic;
using System.Net;
using Kader.DTOs.Cart;
using Kader.DTOs.Orderss;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class ReturnOrdersProfile : Profile
    {
        private IMethods _methods;
        public ReturnOrdersProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<AssignStoreOrderDto, ReturnsOrder>()
                 .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Statusid))
               .ForMember(dest => dest.ReturnsOrderId, opt => opt.MapFrom(src => src.Idorders));
            CreateMap<AcceptReturnsOrdersDto, ReturnsOrder>()
            .ForMember(dis => dis.StatusId, map => map.MapFrom(source => source.StatusID  ))
              
        .ForMember(dis => dis.Comment, map => map.MapFrom(source => source.comment))
        .ForMember(dis => dis.ReturnsOrderId, map => map.MapFrom(source => source.ReturnsOrderId))
        .ForMember(dis => dis.ShippingReturnotoMessge, map => map.MapFrom(source => source.ShippingReturnotoMessge))
        .ForMember(dis => dis.ShippingDate, map => map.MapFrom(source => source.ShippingDate))
        .ForMember(dis => dis.ShippingReturnBool, map => map.MapFrom(source => source.ShippingReturnBool));
            CreateMap<ReturnsOrdersDto, ReturnsOrder>();
            CreateMap<saveReturnsOrdersDto, ReturnsOrder>().ForMember(dis => dis.OrderId, map => map.MapFrom(sourse => sourse.Idordersss));

            
        }
        private void QueryMapper()
        {
            CreateMap<ReturnsOrder, AllReturnsOrdersDto>()
   .ForMember(dest => dest.Idorders, opt => opt.MapFrom(src => src.OrderId))
    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
   .ForMember(dest => dest.TotalPrice, map => map.MapFrom(src => src.Order.TotalPrice))
   .ForMember(dest => dest.TotalTransportShippPrice, map => map.MapFrom(src => src.Order.TotalTransportShippPrice))
   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName))
   .ForPath(dest => dest.ReturnsOrderItemslistDto, map => map.MapFrom(src => src.ReturnsOrderItem))
   .ForMember(dest => dest.InsertDate, map => map.MapFrom(src => src.RequestDate)) 
              .ForMember(dest => dest.ReturnsOrderId, map => map.MapFrom(src => src.ReturnsOrderId));
            CreateMap<ReturnsOrder, ReturnsOrderviewDto>();

            CreateMap<ReturnsOrder, RefundRequestDto>()
                 .ForMember(dest => dest.PaymentId, map => map.MapFrom(src => src.Order.PaymentId))
                  .ForMember(dest => dest.p_id, map => map.MapFrom(src => src.Order.Payment.PId))
                 .ForPath(dest => dest.ReturnsItemsDto, map => map.MapFrom(src => src.ReturnsOrderItem ?? new List<ReturnsOrderItem>()))
            ;
            CreateMap<ReturnsOrder, ReturnsOrdersDto>()
            .ForMember(dest => dest.Itemslist, opt => opt.Ignore()) // Handled dynamically
            .ForMember(dest => dest.ReturnsOrderview, opt => opt.MapFrom(src => src)); // Map view properties

            CreateMap<ReturnsOrder, ReturnOrdersDetailsDto>()
                 .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
                .ForMember(dest => dest.Idorders, map => map.MapFrom(src => src.OrderId))
                 .ForMember(dest => dest.StatusOrderId, map => map.MapFrom(src => src.Order.StatusId))
                 .ForMember(dest => dest.OrderPayment, map => map.MapFrom(src => src.Order.PaymentId))
                .ForMember(dest => dest.ReturnsOrderId, map => map.MapFrom(src => src.ReturnsOrderId))
                .ForMember(dest => dest.TotalPrice, map => map.MapFrom(src => src.Order != null ? src.Order.TotalPrice : 0))
                .ForMember(dest => dest.TotalTransportShippPrice, map => map.MapFrom(src => src.Order != null ? src.Order.TotalTransportShippPrice : 0))
                .ForMember(dest => dest.Status, map => map.MapFrom(src => src.Status != null ? src.Status.StatusName : "Unknown"))
                .ForPath(dest => dest.ReturnsOrderItemsDetail, map => map.MapFrom(src => src.ReturnsOrderItem ?? new List<ReturnsOrderItem>()))
                  .ForPath(dest => dest.OrderStatusw, map => map.MapFrom(src => src.ReturnsOrderStatus))

                .ForMember(dest => dest.InsertDate, map => map.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.UserName, map => map.MapFrom(src => src.InsertedByNavigation != null ? src.InsertedByNavigation.UserName : string.Empty))
                .ForMember(dest => dest.FirstName, map => map.MapFrom(src => src.InsertedByNavigation != null ? src.InsertedByNavigation.FirstName : string.Empty))
                 .ForMember(dest => dest.AddressDto, map => map.MapFrom(src => src.Order.Address))
                .ForMember(dest => dest.LastName, map => map.MapFrom(src => src.InsertedByNavigation != null ? src.InsertedByNavigation.LastName : string.Empty));

        }
    }

}
