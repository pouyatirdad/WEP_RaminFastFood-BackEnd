using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class FaqCategory : BaseEntity
    {
        [Display(Name = "نام دسته")]
        [MaxLength(400, ErrorMessage = "نام دسته باید از 400 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد کنید")]
        public string Title { get; set; }
        public ICollection<Faq> Faqs { get; set; }
    }
}
