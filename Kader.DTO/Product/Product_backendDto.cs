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
    public class Product_backendDto
    {
        public List<ProductViewDto> ExistingItems { get; set; }
        public List<ApiProductDto> ExistingItems1 { get; set; }
        public ProductDto NewItem { get; set; }

    }
}