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

namespace Kader.DTOs.Product
{
    public class ApiEditProductDto
    {

        public int catID { get; set; }
        public string Unit_Name { get; set; }
        public string Image { get; set; }
        public int ProductId { get; set; }
        public int p_id { get; set; }
        public int ApiProdID { get; set; } 
        public string title { get; set; } 
        public decimal price { get; set; }
        public string barcode { get; set; }
        public string SubCatogryTitle { get; set; }
        public string Description { get; set; }
        public double stock { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? MinQuantityToShipJomla { get; set; }
        public int? typeGomlaOrQt3 { get; set; } //1 gomla  2 qt3
        public int? ColorId { get; set; }
        public bool? ApearInHomePage { get; set; }
        public bool? SpecialOrder { get; set; }
        public double? AfterDiscount { get; set; }
        public DateTime? discountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public string unites1 { get; set; }
        public int? tax_ID { get; set; }
        public int? unit_Id { get; set; }
        public IList<ShippingPriceDto> ShippingPriceDto { get; set; }
        public double? ShippingPrice { get; set; }
        public double Quantity_user { get; set; }
        public IList<ProductImgDto> ProductImgDto { get; set; }
        public IList<LogPriceDto> LogPriceDto { get; set; }
        
        public ApiEditProductDto()
        {
            ShippingPriceDto = new List<ShippingPriceDto>();
            ProductImgDto = new List<ProductImgDto>();
            LogPriceDto = new List<LogPriceDto>();
        }
        
            public int? CatTypeId { get; set; }
        public decimal special { get; set; }
        public List<int> categories { get; set; }
        public List<UnitDto> unites { get; set; }
    }

   

 
}