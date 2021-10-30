using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class Product : BaseEntity
    {
        [Display(Name = "عنوان محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیح کوتاه")]
        [DataType(DataType.MultilineText)]
        [MaxLength(2000, ErrorMessage = "{0} باید کمتر از 2000 کارکتر باشد")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "تصویر محصول")]
        public string Image { get; set; }

        [Display(Name = "امتیاز محصول")]
        public int Rate { get; set; }
        public int? ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public ICollection<ProductMainFeature> ProductMainFeatures { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<Discount> Discounts { get; set; }
        public ICollection<ProductFeatureValue> ProductFeatureValues { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
