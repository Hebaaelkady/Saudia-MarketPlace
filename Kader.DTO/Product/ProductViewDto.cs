using Kader.Data.DataAccessLayer.Entities;
using Kader.DTOs.Catogry;
using Kader.DTOs.ProductImg;
using Kader.DTOs.ShippingPrice;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kader.DTOs.Product
{
    public class ProductViewDto
    {
        public int ApiProdID { get; set; }
        public int ProductId { get; set; }
        public string CatogryName { get; set; }
        public string SubCatogryTitle { get; set; }
        public string barcode { get; set; }
        public double? AfterDiscount { get; set; }
        public double? BeforeDiscount { get; set; }
        public DateTime? discountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public double? MinQuantityToShipJomla { get; set; }
        public string Unit { get; set; }
        public int? QuantityAvailable { get; set; }
        public string TransportMethodId { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? ShippingPrice { get; set; }
        public int? TaxId { get; set; }
        public int? UnitId { get; set; }
        public IList<ShippingPriceDto> ShippingPriceDto { get; set; }
        public ProductViewDto()
        {
            ShippingPriceDto = new List<ShippingPriceDto>();
            ProductImgDto = new List<ProductImgDto>();
        }
        public IList<ProductImgDto> ProductImgDto { get; set; }
         
    }
}