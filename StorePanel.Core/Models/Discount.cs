﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class Discount : BaseEntity
    {
        public string GroupIdentifier { get; set; }
        [Display(Name = "عنوان تخفیف")]
        public string Title { get; set; }
        public int DiscountType { get; set; }
        public long Amount { get; set; }
        public int? OfferId { get; set; }
        [Display(Name = "پیشنهاد")]
        public Offer Offer { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "محصول")]
        public Product Product { get; set; }
        public int? ProductGroupId { get; set; }
        [Display(Name = "دسته")]
        public ProductGroup ProductGroup { get; set; }
        public int? BrandId { get; set; }
        [Display(Name = "برند")]
        public Brand Brand { get; set; }
    }
}
