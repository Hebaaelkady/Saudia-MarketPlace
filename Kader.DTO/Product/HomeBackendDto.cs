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
    public class HomeProductDto
    {
        public int? QuantityAvailable { get; set; }
        public int ProductId { get; set; }
        public string SubCatogryTitle { get; set; }
        public int? CatTypeId { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? MinQuantityToShipJomla { get; set; }
    }
    public class HomeBackendDto
    {
        public int ProductGomla { get; set; }
        public int ProductQta3 { get; set; }

        public int NewReturnOrder { get; set; }
        public int UnderCheckReturnOrder { get; set; }
        public int underShipentReturnOrder { get; set; }
        public int completedReturnOrder { get; set; }
        public int RejectedReturnOrders { get; set; }

        public int NewOrder { get; set; }
        public int AssignedToStores { get; set; }
        public int UnderCheck { get; set; }
        public int AssignedToShipping { get; set; }
        public int InnerunderShipent { get; set; }
        public int NotcompletedOrders { get; set; }
        public int underShipent { get; set; }
        public int completedOrders { get; set; }
        public List<HomeProductDto> HomeProductGomla { get; set; }
        public List<HomeProductDto> HomeProductQta3 { get; set; }
        public HomeBackendDto()
        {
            HomeProductGomla = new List<HomeProductDto>();
            HomeProductQta3 = new List<HomeProductDto>();
        }
    }

}