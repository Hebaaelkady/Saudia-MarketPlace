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
    public class ApiProductDto
    {
         
        public int p_id { get; set; }
        public string title { get; set; } 
        public decimal price { get; set; }
        public decimal special { get; set; }
        public decimal stock { get; set; }
        public string image { get; set; }
        public string image2 { get; set; }
        public List<int> categories { get; set; }
        public decimal tax { get; set; }
        public string tax_label { get; set; }
        public int tax_ID { get; set; }
        public object variations { get; set; } // Use 'object' if you're unsure about the structure 
        public int unit_Id { get; set; }
        public string barcode { get; set; }
        public List<UnitDto> unites { get; set; }
        public object selectedDescriptionIds { get; set; }
        public object ComposeItems { get; set; }
        public string Limit { get; set; }
        public decimal MaxDiscountCash { get; set; }
        public decimal MaxDiscountParcent { get; set; }
        public decimal LastCost { get; set; }
        public decimal MinPriceLevel { get; set; }
    }

    public class UnitDto
    {
        public int id { get; set; }
        public int unit_Id { get; set; }
        public decimal price { get; set; }
        public string barcode { get; set; }
        public string unitName { get; set; }
        public string unitNameEn { get; set; }
        public decimal Ratio { get; set; }
    }

 
}