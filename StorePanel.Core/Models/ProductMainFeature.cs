using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class ProductMainFeature : BaseEntity
    {
        [Display(Name = "ویژگی")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Value { get; set; }
        [Display(Name = "اطلاعات دیگر")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string OtherInfo { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int? SubFeatureId { get; set; }
        public SubFeature SubFeature { get; set; }
        [Display(Name = "تعداد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Quantity { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long Price { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
