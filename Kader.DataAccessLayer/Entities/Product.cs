using System;
using System.Collections.Generic;

namespace Kader.Data.DataAccessLayer.Entities
{
    public partial class Product
    {
        public Product()
        {
            LogPrice = new HashSet<LogPrice>();
            LogQuantity = new HashSet<LogQuantity>();
            OrderItems = new HashSet<OrderItems>();
            ProductImg = new HashSet<ProductImg>();
            ShippingPriceNavigation = new HashSet<ShippingPrice>();
        }

        public int ProductId { get; set; }
        public string SubCatogryTitle { get; set; }
        public string SubCatogrysubTitle { get; set; }
        public int? CatogryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsertedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public double? BeforeDiscount { get; set; }
        public string Description { get; set; }
        public bool? ApearInHomePage { get; set; }
        public bool? SpecialOrder { get; set; }
        public double? MinQuantityToShipJomla { get; set; }
        public int? Unit { get; set; }
        public double? QuantityAvailable { get; set; }
        public int? ColorId { get; set; }
        public int? TransportMethodId { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? ShippingPrice { get; set; }
        public int? CatTypeId { get; set; }
        public double? AfterDiscount { get; set; }
        public DateTime? DiscountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int? ApiProdId { get; set; }
        public int? TaxId { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public string Barcode { get; set; }

        public virtual CatType CatType { get; set; }
        public virtual Catogry Catogry { get; set; }
        public virtual Colors Color { get; set; }
        public virtual TransportMethod TransportMethod { get; set; }
        public virtual Units UnitNavigation { get; set; }
        public virtual ICollection<LogPrice> LogPrice { get; set; }
        public virtual ICollection<LogQuantity> LogQuantity { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<ProductImg> ProductImg { get; set; }
        public virtual ICollection<ShippingPrice> ShippingPriceNavigation { get; set; }
    }
}
