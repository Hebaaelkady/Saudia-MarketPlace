using Kader.DTOs.Ads;
using Kader.DTOs.BannerImgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Product
{
    public class HomeVarDto
    {
        public int p_id { get; set; }
        public int ApiProdID { get; set; }
        public string title { get; set; }
        public string Description { get; set; }
        public bool? SpecialOrder { get; set; } 
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int? CatogryId { get; set; }
        public string SubCatogryTitle { get; set; }
        public double? BeforeDiscount { get; set; }
        public double? AfterDiscount { get; set; }
        public string Attribute { get; set; }
        public string Image { get; set; }
        public int? CatTypeId { get; set; }
    }
    public class HomeDto
    {
        public List<HomeVarDto> specialProductGomla { get; set; }
        public List<HomeVarDto> specialProductQt3 { get; set; }
        public List<HomeVarDto> GoodPriceGomla { get; set; }
        public List<HomeVarDto> GoodPriceQt3 { get; set; }
        public List<BannerImgsDto> BannerImgsDto { get; set; }
        public  AdsDto  AdsDto { get; set; }
        public HomeDto()
        {
            specialProductGomla = new List<HomeVarDto>();
            specialProductQt3 = new List<HomeVarDto>();
            GoodPriceGomla = new List<HomeVarDto>();
            GoodPriceQt3 = new List<HomeVarDto>();
            BannerImgsDto = new List<BannerImgsDto>();
            AdsDto = new  AdsDto ();
        }
    }
}
