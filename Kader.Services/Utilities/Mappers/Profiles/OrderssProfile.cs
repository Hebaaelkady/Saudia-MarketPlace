using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Cart;
using Kader.DTOs.Orderss;
using Kader.DTOs.Product;
using Kader.DTOs.ReturnOrders;
using Kader.DTOs.ReturnsOrderItem;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using System;
using System.Linq;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class OrderssProfile : Profile
    {
        private IMethods _methods;
        public OrderssProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<Orderss, OrderBackendDto>(); 
            CreateMap<OrderssDto, Orderss>();
            CreateMap<MsgDto, Orderss>();
            CreateMap<Checkout_frontDto, Orderss>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.id)) ;
            CreateMap<AssignStoreOrderDto, Orderss>()
                 .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Statusid))
               .ForMember(dest => dest.Idorders, opt => opt.MapFrom(src => src.Idorders));
            CreateMap<Checkout_frontDto, ApiOrderDto>()
           .ForMember(dest => dest.Param, opt => opt.MapFrom(src => src.Itemslist))
         .ForMember(dest => dest.CustomerID, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.UserId1) ? 0 : int.Parse(src.UserId1)))
           .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.CouponDiscount))
           .ForMember(dest => dest.PaymentType_ID, opt => opt.MapFrom(src => src.StatusId ?? 0))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => "Order Description")) // Set as needed
       .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.InsertDate.HasValue ? src.InsertDate.Value.ToString("HH:mm:ss") : string.Empty))  
           .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.FirstName))
           .ForMember(dest => dest.UsrRefNbr, opt => opt.MapFrom(src => src.OrdersNo)) // Set as needed
           .ForMember(dest => dest.DateInvoice, opt => opt.MapFrom(src => src.InsertDate ))
           .ForMember(dest => dest.CustomerMobile, opt => opt.MapFrom(src => src.UserName)) 
           // Set as needed
            ;
            CreateMap<Itemslist, OrderParam>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceItem))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.P_Id, opt => opt.MapFrom(src => src.ApiProdId))
                  .ForMember(dest => dest.Tax_Id, opt => opt.MapFrom(src => src.TaxId))
                  .ForMember(dest => dest.barcode, opt => opt.MapFrom(src => src.sku))
                    .ForMember(dest => dest.Unit_Id, opt => opt.MapFrom(src => src.UnitId))
                //     .ForMember(dest => dest.P_Id, opt => opt.MapFrom(src => src.ProductId))
                //.ForMember(dest => dest.Unit_Id, opt => opt.MapFrom(src => src.Unit_Id))
                .ForMember(dest => dest.DiscountParcent, opt => opt.MapFrom(src => src.MinQuantityToShipJomla));
            //  .ForMember(dest => dest.DiscountCash, opt => opt.MapFrom(src => src.DiscountCash));
            CreateMap<SaveCheckoutDto, Orderss>();
            ;

        }
        private void QueryMapper()
        {
            CreateMap<Orderss, ReturnsOrdersDto>()
                .ForMember(dest => dest.Idordersss, opt => opt.MapFrom(src => src.Idorders))
                 .ForMember(dest => dest.Itemslist, opt => opt.MapFrom(src => src.OrderItems))
                 .ForMember(dest => dest.ReturnsOrderview, opt => opt.MapFrom(src => src.ReturnsOrder))
                  .ForMember(dest => dest.ReturnsOrderItem, opt => opt.MapFrom(src => src.ReturnsOrder.SelectMany(ro => ro.ReturnsOrderItem)))
                 ; // Flatten and map items
            ;
            //.ForMember(dis => dis.Itemslist, map => map.MapFrom(sourse => sourse.OrderItems));
            //.ForMember(dis => dis.ReturnsOrderDto, map => map.MapFrom(sourse => sourse.ReturnsOrder));

            CreateMap<Orderss, SaveCheckoutDto>();
            CreateMap<Orderss, OrderssDto>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName))
                .ForMember(dest => dest.OrderPayment, opt => opt.MapFrom(src => src.PaymentId)); 
            CreateMap<Orderss, OrderBackendDto>().ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status.StatusName))
                .ForMember(dest => dest.Itemslist, opt => opt.MapFrom(src => src.OrderItems))
                 .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.StoreId))
                .ForMember(dest => dest.OrderPayment, opt => opt.MapFrom(src => src.PaymentId)); // Ensure this mapping exists;
            CreateMap<Orderss, Checkout_frontDto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.PaymentId))

                //.ForMember(dis => dis.Itemslist, map => map.MapFrom(sourse => sourse.OrderItems))
                .ForMember(dis => dis.AddressDto, map => map.MapFrom(sourse => sourse.Address))
                .ForPath(dest => dest.AddressDto.CityName, opt => opt.MapFrom(src => src.Address.CityNavigation.CityName))
                 .ForMember(dis => dis.Idordersss, map => map.MapFrom(sourse => sourse.Idorders))
                  .ForMember(dis => dis.PaymentID, map => map.MapFrom(sourse => sourse.PaymentId))
                    .ForMember(dis => dis.OrdersNo, map => map.MapFrom(sourse => sourse.OrdersNo))
                    .ForMember(dis => dis.LastName, map => map.MapFrom(sourse => sourse.User.LastName))
                     .ForMember(dis => dis.UserName, map => map.MapFrom(sourse => sourse.User.UserName))
                      .ForMember(dis => dis.FirstName, map => map.MapFrom(sourse => sourse.User.FirstName))
                 .ForMember(dis => dis.Itemslist, map => map.MapFrom(sourse => sourse.OrderItems.Select(oi => new Itemslist
                {
                     //CatIdAPI = oi.CatIdApi,
                     typeGomlaOrQt3 = oi.TypeGomlaOrQt3,
                    ProductId = oi.ProductId,
                    SubCatogryTitle = oi.Product.SubCatogryTitle,
                     unit = oi.Product.UnitName,
                    PriceItem = oi.PriceItem,
                    Quantity = oi.Quantity,
                     ProductName = oi.ProductName,
                     date = oi.InsertDate ?? DateTime.UtcNow.AddHours(3),
                    ShippingPrice = oi.ShippingPrice,
                     TaxId = oi.Product.TaxId,UnitId=oi.Product.UnitId,
                     // ShippingPriceNavigation = oi.Product.ShippingPriceNavigation,
                     Colornme = oi.Color != null ? oi.Color.ColorDegree : "",
                       //ApiProdId = oi.Product.ApiProdId.Value,
                     OrderId = oi.OrderId
                }))); ; ;

            CreateMap<Orderss, OrderDetailsDto>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.StoreName))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.PaymentId))
                .ForMember(dis => dis.Itemslist, map => map.MapFrom(sourse => sourse.OrderItems))
                .ForMember(dis => dis.AddressDto, map => map.MapFrom(sourse => sourse.Address))
                .ForMember(dis => dis.CountryCode, map => map.MapFrom(sourse => sourse.User.CountryCode))
                .ForPath(dest => dest.AddressDto.CityName, opt => opt.MapFrom(src => src.Address.CityNavigation.CityName))
                 .ForMember(dis => dis.Idordersss, map => map.MapFrom(sourse => sourse.Idorders))
                  .ForMember(dis => dis.PaymentID, map => map.MapFrom(sourse => sourse.PaymentId))
                    .ForMember(dis => dis.OrdersNo, map => map.MapFrom(sourse => sourse.OrdersNo))
                    .ForMember(dis => dis.InsideShippingUserFirstName, map => map.MapFrom(sourse => sourse.InsideShippingUserNavigation.FirstName))
                    .ForMember(dis => dis.InsideShippingUserLastName, map => map.MapFrom(sourse => sourse.InsideShippingUserNavigation.LastName))
                    .ForMember(dis => dis.InsideShippingUserPhoneNumber, map => map.MapFrom(sourse => sourse.InsideShippingUserNavigation.PhoneNumber))
                    .ForMember(dis => dis.LastName, map => map.MapFrom(sourse => sourse.User.LastName))
                     .ForMember(dis => dis.UserName, map => map.MapFrom(sourse => sourse.User.UserName))
                      .ForMember(dis => dis.FirstName, map => map.MapFrom(sourse => sourse.User.FirstName))
                 .ForMember(dis => dis.Itemslist, map => map.MapFrom(sourse => sourse.OrderItems.Select(oi => new Itemslist
                 {
                     ProductId = oi.ProductId,
                     sku = oi.Product.Barcode,

                     SubCatogryTitle = oi.Product.SubCatogryTitle,
                        unit = oi.Unit != null ? oi.Product.UnitNavigation.UnitName : "",
                     
                     PriceItem = oi.PriceItem,
                     Quantity = oi.Quantity,
                     ProductName = oi.ProductName,
                     date = oi.InsertDate ?? DateTime.UtcNow.AddHours(3),
                     ShippingPrice = oi.ShippingPrice,
                     //TaxId = oi.Product.TaxId,
                     //UnitId = oi.Product.UnitId,
                     // ShippingPriceNavigation = oi.Product.ShippingPriceNavigation,
                     Colornme = oi.Color != null ? oi.Color.ColorDegree : "",
                    // ApiProdId = oi.Product.ApiProdId.Value,
                     OrderId = oi.OrderId
                 }))); ; ;

        }
    }

}
