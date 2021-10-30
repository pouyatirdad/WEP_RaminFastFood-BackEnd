using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class Faq : BaseEntity
    {
        [Display(Name = "سوال")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Question { get; set; }
        [Display(Name = "جواب")]
        [Required(ErrorMessage = "لطفا نام دسته را وارد کنید")]
        [DataType(DataType.MultilineText)]
        public string Answer { get; set; }
        public int FaqCategoryId { get; set; }
        public FaqCategory FaqCategory { get; set; }
    }
}
