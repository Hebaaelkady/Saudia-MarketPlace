using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Cart;
using Kader.DTOs.Catogry;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.OrderItems; 
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class OrderItemsProfile : Profile
    {
        private IMethods _methods;
        public OrderItemsProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<Itemslist, OrderItems>()
                .ForMember(dis => dis.Product, map => map.MapFrom(sourse => sourse.Product))
                 .ForMember(dis => dis.Color, map => map.MapFrom(sourse => sourse.Colors))
                ;


        }
        private void QueryMapper()
        {
            CreateMap<OrderItems, Itemslist>().ForMember(dis => dis.Product, map => map.MapFrom(sourse => sourse.Product))
                .ForMember(dis => dis.unit, map => map.MapFrom(sourse => sourse.Product.UnitName))
                ;
            CreateMap<OrderItems, ItemslistOrderDto>()
                .ForMember(dis => dis.ProductName, map => map.MapFrom(sourse => sourse.ProductName));
            CreateMap<OrderItems, OrderItemsDto>()

;        }
    }

}
