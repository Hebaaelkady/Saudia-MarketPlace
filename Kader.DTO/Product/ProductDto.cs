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
    using Kader.DTOs.LogQuantity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductDto : IValidatableObject
    {
        public string CatogryName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string barcode { get; set; }

        public int ApiProdID { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string SubCatogryTitle { get; set; }

        public string SubCatogrysubTitle { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int? CatogryId { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [RegularExpression(@"^[0-9]*(\.[0-9]+)?$", ErrorMessage = "مسموح بالأرقام والأرقام العشرية فقط.")]
        public double? BeforeDiscount { get; set; }

        public double? AfterDiscount { get; set; }

        public int? TaxId { get; set; }
        public string UnitName { get; set; }
        public DateTime? discountBeginDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public string Description { get; set; }
        public bool? ApearInHomePage { get; set; }
        public bool? SpecialOrder { get; set; }
        public double? MinQuantityToShipJomla { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int? Unit { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "مسموح ارقام فقط.")]
        public int? QuantityAvailable { get; set; }
        public int? QuantityAdded { get; set; }
        
        public int? ColorId { get; set; }
        public int? TransportMethodId { get; set; }
        public double? MaxQuantityToShipQta3a { get; set; }
        public double? ShippingPrice { get; set; }
        public int? CatTypeId { get; set; }
        public IList<ShippingPriceDto> ShippingPriceDto { get; set; }
        public IList<ProductImgDto> ProductImgDto { get; set; }
        public IList<LogPriceDto> LogPriceDto { get; set; }
        public IList<LogQuantityDto> LogQuantityDto { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public ProductDto()
        {
            ShippingPriceDto = new List<ShippingPriceDto>();
            ProductImgDto = new List<ProductImgDto>();
            LogPriceDto = new List<LogPriceDto>();
            LogQuantityDto = new List<LogQuantityDto>();
        }

        // ✅ Custom Validation: Ensure BeforeDiscount > AfterDiscount
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BeforeDiscount.HasValue && AfterDiscount.HasValue)
            {
                if (BeforeDiscount <= AfterDiscount)
                {
                    yield return new ValidationResult("يجب أن يكون السعر قبل الخصم أكبر من السعر بعد الخصم.", new[] { "BeforeDiscount" });
                }
            }
        }
    }

}