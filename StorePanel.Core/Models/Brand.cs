using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class Brand : BaseEntity
    {
        [Display(Name = "لوگو")]
        public string Logo { get; set; }
        [Display(Name = "نام برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        public ICollection<ProductGroupBrand> ProductGroupBrands { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Discount> Discounts { get; set; }
    }
}
