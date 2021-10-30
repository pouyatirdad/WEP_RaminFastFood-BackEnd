using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class SubFeature : BaseEntity
    {
        [Display(Name = "میزان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Value { get; set; }
        [Display(Name = "اطلاعات دیگر")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string OtherInfo { get; set; }
        public int? FeatureId { get; set; }
        public Feature Feature { get; set; }
        public ICollection<ProductMainFeature> ProductMainFeatures { get; set; }
        public ICollection<ProductFeatureValue> ProductFeatureValues { get; set; }
    }
}
