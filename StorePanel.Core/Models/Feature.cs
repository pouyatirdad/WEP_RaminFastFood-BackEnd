using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class Feature : BaseEntity
    {
        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(600, ErrorMessage = "{0} باید کمتر از 600 کارکتر باشد")]
        public string Title { get; set; }
        public ICollection<ProductGroupFeature> ProductGroupFeatures { get; set; }
        public ICollection<SubFeature> SubFeatures { get; set; }
        public ICollection<ProductMainFeature> ProductMainFeatures { get; set; }
        public ICollection<ProductFeatureValue> ProductFeatureValues { get; set; }
    }
}
