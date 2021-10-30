using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class ProductGroup : BaseEntity
    {
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Title { get; set; }
        public string Image { get; set; }
        public ICollection<Product> Products { get; set; }
        public int? ParentId { get; set; }
        public virtual ProductGroup Parent { get; set; }
        public virtual ICollection<ProductGroup> Children { get; set; }
        public ICollection<ProductGroupFeature> ProductGroupFeatures { get; set; }
        public ICollection<ProductGroupBrand> ProductGroupBrands { get; set; }
        public ICollection<Discount> Discounts { get; set; }
    }
}
