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
    public class SearchProductDto
    {
        public int ApiProdID { get; set; } 
        public int ProductId { get; set; } 
        public string SubCatogryTitle { get; set; }
        public int? CatTypeId { get; set; }
    }
}