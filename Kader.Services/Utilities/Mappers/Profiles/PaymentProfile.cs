using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.Payment; 
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class PaymentProfile : Profile
    {
        private IMethods _methods;
        public PaymentProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<PaymentDto, Payment>();
            CreateMap<RefundResponseDto, Payment>()
                   .ForMember(dis => dis.Id, map => map.MapFrom(sourse => sourse.Id))
             .ForMember(dis => dis.Type, map => map.MapFrom(sourse => sourse.Source.Type))
               .ForMember(dis => dis.Name, map => map.MapFrom(sourse => sourse.Source.Name))
                .ForMember(dis => dis.Number, map => map.MapFrom(sourse => sourse.Source.Number))
                 .ForMember(dis => dis.GatewayId, map => map.MapFrom(sourse => sourse.Source.GatewayId))
                  .ForMember(dis => dis.ReferenceNumber, map => map.MapFrom(sourse => sourse.Source.ReferenceNumber))
                   .ForMember(dis => dis.Token, map => map.MapFrom(sourse => sourse.Source.Token))
                    .ForMember(dis => dis.Message, map => map.MapFrom(sourse => sourse.Source.Message))
                     .ForMember(dis => dis.ResponseCode, map => map.MapFrom(sourse => sourse.Source.ResponseCode))
                      .ForMember(dis => dis.AuthorizationCode, map => map.MapFrom(sourse => sourse.Source.AuthorizationCode))
                       .ForMember(dis => dis.IssuerName, map => map.MapFrom(sourse => sourse.Source.IssuerName))
                        .ForMember(dis => dis.IssuerCardType, map => map.MapFrom(sourse => sourse.Source.IssuerCardType))
                         .ForMember(dis => dis.IssuerCardCategory, map => map.MapFrom(sourse => sourse.Source.IssuerCardCategory))
                         .ForMember(dis => dis.Description, map => map.MapFrom(sourse => sourse.Message1))
                         .ForMember(dis => dis.Status, map => map.MapFrom(sourse => sourse.Status))
                         .ForMember(dis => dis.Refunded, map => map.MapFrom(sourse => sourse.Refunded))
                          .ForMember(dis => dis.RefundedFormat, map => map.MapFrom(sourse => sourse.RefundedFormat))
                           .ForMember(dis => dis.Ip, map => map.MapFrom(sourse => sourse.Ip))
                           .ForMember(dis => dis.RefundSuccess, map => map.MapFrom(sourse => sourse.IsSuccess))
                         ;
        }
        private void QueryMapper()
        {
            CreateMap<Payment, PaymentDto>() ;
            
        }
    }

}
