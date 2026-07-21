using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Catogry;
using Kader.DTOs.ProductImg;
using Kader.DTOs.LogPrice;
using Kader.DTOs.ShippingPrice;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Kader.DTOs.Cart;

namespace Kader.DTOs.Product
{
    public class Product_frontEndDto
    {
        public int ApiProdID { get; set; }
        public string CatogryName { get; set; }
        public int ProductId { get; set; }
        public int? CatogryId { get; set; }
        public string SubCatogryTitle { get; set; }
public double? BeforeDiscount { get; set; }
        public double? AfterDiscount { get; set; }

        public int? QuantityAvailable { get; set; }
        public DateTime? discountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public string Description { get; set; }
        public bool? ApearInHomePage { get; set; }
        public bool? SpecialOrder { get; set; }
        public int? MinQuantityToShipJomla { get; set; }
        public int? MaxQuantityToShipQta3a { get; set; }
        public int? typeGomlaOrQt3 { get; set; }
        public int? CatTypeId { get; set; }
        public decimal Price { get; set; }
        public int? Unit { get; set; } 
        public int? ColorName { get; set; }  
        public double? ShippingPrice { get; set; }
        public int? Quantity { get; set; }
        public string ProductImgName { get; set; }
        public ProductItemDto ProductItemDto { get; set; }
        public IList<ProductImgDto> ProductImgDto { get; set; }
        public List<ShippingPriceDto> ShippingPriceDto { get; set; }
        public Product_frontEndDto()
        {
            ProductItemDto = new ProductItemDto();
            ProductImgDto = new List<ProductImgDto>();
            ShippingPriceDto = new List<ShippingPriceDto>();
        }

    }
}