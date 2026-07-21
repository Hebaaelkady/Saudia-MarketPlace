using Kader.DTOs.Product;
using Kader.DTOs.Colors;
using Kader.DTOs.ShippingPrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kader.DTOs.Cart
{
    public class Itemslist
    {
        public double? AfterDiscount { get; set; }

        public double  BeforeDiscount { get; set; }

        public decimal Price_enter { get; set; }
        public decimal Quantity_user { get; set; }
        public int ProductId { get; set; }
        public int ApiProdId { get; set; }
        public string SubCatogryTitle { get; set; }
        public string sku { get; set; }
        public string unit { get; set; }
        public string ProductName { get; set; }
        public string ProductImgName { get; set; }
        public double? PriceItem { get; set; }
        public int Quantity { get; set; }
        public int? MinQuantityToShipJomla { get; set; }
        public int? MaxQuantityToShipQta3a { get; set; }
        public int? typeGomlaOrQt3 { get; set; } //1 gomla  2 qt3
        public DateTime date { get; set; }
        public int? QuantityAvailable { get; set; }
        public double? ShippingPrice { get; set; }
        public virtual List<ShippingPriceDto> ShippingPriceNavigation { get; set; }
        public string Colornme { get; set; }
        public string Action { get; set; }
        public virtual ProductDto Product { get; set; }
        public virtual ColorsDto Colors { get; set; }
        public int OrderId { get; set; }
        public double? AnyShippingPrice { get; set; }
        public int? TaxId { get; set; }
        public int? UnitId { get; set; }
        public int? CatIdAPI { get; set; }
        public decimal stock { get; set; }

    }
    public class ProductItemDto
    {
        public List<Itemslist> Itemslist { get; set; } // Now a list of items
    }
}
