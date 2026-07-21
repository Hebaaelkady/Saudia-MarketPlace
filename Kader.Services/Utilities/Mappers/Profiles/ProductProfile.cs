using AutoMapper;
using Kader.Data.DataAccessLayer.Entities;
using Kader.Data.DataAccessLayer.Entities.StoredProcedure;
using Kader.DTOs.Cart;
using Kader.DTOs.OrderItems;
using Kader.DTOs.Product;
using Kader.Infrastructure.Shared.Implementations;
using Kader.Infrastructure.Shared.Interfaces;
using System.Linq;

namespace Kader.Services.Utilities.Mappers.Profiles
{
 
    public class ProductProfile : Profile
    {
        private IMethods _methods;
        public ProductProfile()
        {
            _methods = new Methods();
            CommandMapper();
            QueryMapper();
        }

        private void CommandMapper()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<HomeDto, Product>();
            CreateMap<Product_frontEndDto, Product>();
            CreateMap<ApiEditProductDto, Product>()
                .ForMember(dis => dis.TaxId, map => map.MapFrom(sourse => sourse.tax_ID))
  .ForMember(dis => dis.UnitId, map => map.MapFrom(sourse => sourse.unit_Id))
    .ForMember(dis => dis.Barcode, map => map.MapFrom(sourse => sourse.barcode))
              .ForMember(dis => dis.UnitName, map => map.MapFrom(sourse => sourse.Unit_Name));

        }
        private void QueryMapper()
        {
            CreateMap<Product, HomeProductDto>();

            CreateMap<Product, HomeVarDto>().ForMember(dis => dis.Image, map => map.MapFrom(sourse => sourse.ProductImg.FirstOrDefault().ProductImgName));
            CreateMap<Product, Product_frontEndDto>()
                  .ForMember(dis => dis.ProductImgDto, map => map.MapFrom(sourse => sourse.ProductImg))
                  .ForMember(dis => dis.typeGomlaOrQt3, map => map.MapFrom(sourse => sourse.CatTypeId))
                  .ForMember(dis => dis.ShippingPriceDto, map => map.MapFrom(sourse => sourse.ShippingPriceNavigation))
                  .ForMember(dis => dis.CatogryName, map => map.MapFrom(sourse => sourse.Catogry.CatogryName))
                     .ForMember(dis => dis.CatogryId, map => map.MapFrom(sourse => sourse.Catogry.CatogryId)); ;
            CreateMap<Product, ProductDto>()
                 .ForMember(dis => dis.LogQuantityDto, map => map.MapFrom(sourse => sourse.LogQuantity))
            .ForMember(dis => dis.ShippingPriceDto, map => map.MapFrom(sourse => sourse.ShippingPriceNavigation))
 .ForMember(dis => dis.ProductImgDto, map => map.MapFrom(sourse => sourse.ProductImg))
  .ForMember(dis => dis.UnitName, map => map.MapFrom(sourse => sourse.UnitNavigation.UnitName))
    .ForMember(dis => dis.barcode, map => map.MapFrom(sourse => sourse.Barcode))
                  .ForMember(dis => dis.CatogryName, map => map.MapFrom(sourse => sourse.Catogry.CatogryName))
                  .ForMember(dis => dis.LogPriceDto, map => map.MapFrom(sourse => sourse.LogPrice));
            CreateMap<Product, OrderItemsDto>();
            CreateMap<Product, OrderItemsDto>();
            CreateMap<Product, SearchProductDto>();
            CreateMap<Product, ProductViewDto>().ForMember(dis => dis.TransportMethodId, map => map.MapFrom(sourse => sourse.TransportMethod.TransportMethodName))
            .ForMember(dis => dis.ShippingPriceDto, map => map.MapFrom(sourse => sourse.ShippingPriceNavigation))
                  .ForMember(dis => dis.ProductImgDto, map => map.MapFrom(sourse => sourse.ProductImg))
                  .ForMember(dis => dis.CatogryName, map => map.MapFrom(sourse => sourse.Catogry.CatogryName)) ;
            CreateMap<Product, ApiEditProductDto>()
                 .ForMember(dis => dis.tax_ID, map => map.MapFrom(sourse => sourse.TaxId)).
                  ForMember(dis => dis.unit_Id, map => map.MapFrom(sourse => sourse.UnitNavigation.UnitId))

                   .ForMember(dis => dis.Unit_Name, map => map.MapFrom(sourse => sourse.UnitNavigation.UnitName))
             .ForMember(dis => dis.ShippingPriceDto, map => map.MapFrom(sourse => sourse.ShippingPriceNavigation))
                  .ForMember(dis => dis.ProductImgDto, map => map.MapFrom(sourse => sourse.ProductImg))
                    .ForMember(dis => dis.LogPriceDto, map => map.MapFrom(sourse => sourse.LogPrice))
                    .ForMember(dis => dis.CatTypeId, map => map.MapFrom(sourse => sourse.CatTypeId))
                     .ForMember(dis => dis.barcode, map => map.MapFrom(sourse => sourse.Barcode))
                     .ForMember(dis => dis.price, map => map.MapFrom(sourse => sourse.BeforeDiscount))
                     .ForMember(dis => dis.stock, map => map.MapFrom(sourse => sourse.QuantityAvailable))
                      .ForMember(dis => dis.catID, map => map.MapFrom(sourse => sourse.CatogryId))
                       .ForMember(dis => dis.Image, map => map.MapFrom(sourse => sourse.ProductImg.FirstOrDefault().ProductImgName))
                  ;
           // CreateMap<Itemslist, ApiEditProductDto>().ForMember(dis => dis.title, map => map.MapFrom(sourse => sourse.SubCatogryTitle));
            CreateMap<ApiEditProductDto, Itemslist>().ForMember(dis => dis.SubCatogryTitle, map => map.MapFrom(sourse => sourse.title))
                .ForMember(dis => dis.QuantityAvailable, map => map.MapFrom(sourse => sourse.stock))
                .ForMember(dis => dis.PriceItem, map => map.MapFrom(sourse => sourse.price))
                 .ForMember(dis => dis.sku, map => map.MapFrom(sourse => sourse.barcode))
                 ;
            CreateMap<Product, Itemslist>()
                .ForMember(dis => dis.UnitId, map => map.MapFrom(sourse => sourse.UnitNavigation.UnitId))
                 .ForMember(dis => dis.unit, map => map.MapFrom(sourse => sourse.UnitNavigation.UnitName))
               //.ForMember(dis => dis.QuantityAvailable, map => map.MapFrom(sourse => sourse.stock))
               //.ForMember(dis => dis.PriceItem, map => map.MapFrom(sourse => sourse.price))
               // .ForMember(dis => dis.sku, map => map.MapFrom(sourse => sourse.barcode))
                ;
        }
    }

}
