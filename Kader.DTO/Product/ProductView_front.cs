using Kader.DTOs.Catogry;
using Kader.DTOs.LogPrice;
using Kader.DTOs.ProductImg;
using Kader.DTOs.ShippingPrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Product
{
    public class ProductView_front
    {
        public ApiEditProductDto Productsitem { get; set; }
        public IList<ApiEditProductDto> Products { get; set; }
        public IList<ApiCatogryDto> ApiCatogryDto { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } // Add this property
        public string SortOrder { get; set; }
        public string SelectedCategoryIds { get; set; }
        public ProductView_front()
        {
            ApiCatogryDto = new List<ApiCatogryDto>();
            Products = new List<ApiEditProductDto>();
             
        }
    }
}
