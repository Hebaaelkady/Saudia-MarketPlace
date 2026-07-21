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
    public class MergedAPIDto
    {

         
        public int ProductId { get; set; }
        public int p_id { get; set; }
        public int ApiProdID { get; set; } 
        public string title { get; set; } 
        public decimal price { get; set; }
        public string SubCatogryTitle { get; set; }
        public string Description { get; set; }
        public decimal stock { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? MinQuantityToShipJomla { get; set; } 
     
        public double? AfterDiscount { get; set; }
        public string Image { get; set; }
        public int? unit_Id { get; set; }
        public double? ShippingPrice { get; set; }
            public int? CatTypeId { get; set; }
        public List<int> categories { get; set; }
       
        
    }

   

 
}